﻿using System.Runtime.CompilerServices;
using Dumbogram.Api.Application.Files.Controllers.Dto;
using Dumbogram.Api.Application.Files.Services.FileFormats;
using Dumbogram.Api.Application.Files.Services.StorageWriter;
using Dumbogram.Api.Application.Users.Services;
using Dumbogram.Api.Models.Files;
using Dumbogram.Api.Models.Files.FileTypes;
using SkiaSharp;
using File = Dumbogram.Api.Models.Files.File;

namespace Dumbogram.Api.Application.Files.Services;

public class UploadService
{
    private readonly FileService _fileService;
    private readonly FilesGroupService _filesGroupService;
    private readonly FileStorageService _fileStorageService;
    private readonly FileTransferService _fileTransferService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserResolverService _userResolverService;

    public UploadService(
        FileTransferService fileTransferService,
        IHttpContextAccessor httpContextAccessor,
        FileService fileService,
        FilesGroupService filesGroupService,
        FileStorageService fileStorageService,
        UserResolverService userResolverService
    )
    {
        _fileTransferService = fileTransferService;
        _fileService = fileService;
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
        var filesResults = await _fileTransferService.UploadSmallFiles<FilePhoto>(request, writer, uploadsLimit);

        var uploadedFiles = filesResults.GetSucceededValues().ToList();

        foreach (var file in uploadedFiles)
        {
            await using var imageFile = _fileStorageService.ReadFile(file.StoredFileName);

            using var bitmap = SKBitmap.Decode(imageFile);
            var width = bitmap.Width;
            var height = bitmap.Height;

            file.Metadata = new FilePhotoMetadata
            {
                Width = width,
                Height = height
            };
        }

        await _fileService.AddFilesRange(uploadedFiles);
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
        var filesResults = await _fileTransferService.UploadLargeFiles<File>(request, writer, uploadsLimit);

        var uploadedFiles = filesResults.GetSucceededValues();

        await _fileService.AddFilesRange(uploadedFiles);
        await _filesGroupService.AddFilesRangeToFilesGroup(group, uploadedFiles);

        var response = FilesUploadResponse.Parse(filesResults);
        return response;
    }
}