using System.Runtime.CompilerServices;
using Dumbogram.Api.Application.Files.Controllers.Dto;
using Dumbogram.Api.Application.Files.Services.FileFormats;
using Dumbogram.Api.Application.Files.Services.StorageWriter;
using Dumbogram.Api.Application.Users.Services;
using Dumbogram.Api.Persistence.Context.Application.Entities.Files;
using Dumbogram.Api.Persistence.Context.Application.Enumerations;
using SkiaSharp;
using FileMetadata = Dumbogram.Api.Persistence.Context.Application.Entities.Files.FileMetadata;

namespace Dumbogram.Api.Application.Files.Services;

public class UploadService
{
    private readonly FileRecordService _fileRecordService;
    private readonly FilesGroupService _filesGroupService;
    private readonly FileStorageService _fileStorageService;
    private readonly FileTransferService _fileTransferService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserResolverService _userResolverService;

    public UploadService(
        FileTransferService fileTransferService,
        IHttpContextAccessor httpContextAccessor,
        FileRecordService fileRecordService,
        FilesGroupService filesGroupService,
        FileStorageService fileStorageService,
        UserResolverService userResolverService
    )
    {
        _fileTransferService = fileTransferService;
        _fileRecordService = fileRecordService;
        _httpContextAccessor = httpContextAccessor;
        _filesGroupService = filesGroupService;
        _fileStorageService = fileStorageService;
        _userResolverService = userResolverService;
    }

    public async Task<FilesUploadResponse> UploadIntoGroup(FilesGroup group)
    {
        return group.GroupType switch
        {
            FilesGroupType.AttachedPhotos => await UploadPhotos(group),
            FilesGroupType.AttachedDocuments => await UploadDocuments(group),
            _ => throw new SwitchExpressionException()
        };
    }

    private async Task<FilesUploadResponse> UploadPhotos(FilesGroup group)
    {
        var writer = new StorageWriter.StorageWriter()
            .SetFileFormatValidationPolicy(FileFormatValidationPolicy.ValidateByExtensionAndSignature)
            .AddPermittedFileFormats(FileFormatGroups.Photo)
            .SetFileLengthLimit(50_000);

        var filesQuantityLimit = FilesGroupLimits.GetFilesQuantityLimit(group.GroupType);
        var uploadsLimit = filesQuantityLimit - group.Files.Count();

        var request = _httpContextAccessor.HttpContext!.Request;
        var filesResults = await _fileTransferService.UploadSmallFiles(request, writer, uploadsLimit);

        var uploadedFiles = filesResults.GetSucceededValues().ToList();

        foreach (var file in uploadedFiles)
        {
            await using var imageFile = _fileStorageService.ReadFile(file.StoredFileName);

            using var bitmap = SKBitmap.Decode(imageFile);
            var width = bitmap.Width;
            var height = bitmap.Height;

            file.Type = FileType.Photo;
            file.Metadata = FileMetadata.Image(new ImageFileMetadataContent
            {
                Width = width,
                Height = height
            });
        }

        await _fileRecordService.AddFilesRange(uploadedFiles);
        await _filesGroupService.AddFilesRangeToFilesGroup(group, uploadedFiles);

        var response = FilesUploadResponse.Parse(filesResults);
        return response;
    }


    private async Task<FilesUploadResponse> UploadDocuments(FilesGroup group)
    {
        var writer = new StorageWriter.StorageWriter()
            .SetFileFormatValidationPolicy(FileFormatValidationPolicy.DoNotValidate);

        var filesQuantityLimit = FilesGroupLimits.GetFilesQuantityLimit(group.GroupType);
        var uploadsLimit = filesQuantityLimit - group.Files.Count();

        var request = _httpContextAccessor.HttpContext!.Request;
        var filesResults = await _fileTransferService.UploadLargeFiles(request, writer, uploadsLimit);

        var uploadedFiles = filesResults.GetSucceededValues();

        await _fileRecordService.AddFilesRange(uploadedFiles);
        await _filesGroupService.AddFilesRangeToFilesGroup(group, uploadedFiles);

        var response = FilesUploadResponse.Parse(filesResults);
        return response;
    }
}