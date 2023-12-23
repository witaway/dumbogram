using System.Runtime.CompilerServices;
using Dumbogram.Api.Common.Classes;
using Dumbogram.Api.Domain.Services.Users;
using Dumbogram.Api.Infrastructure.Files;
using Dumbogram.Api.Infrastructure.Files.FileFormats;
using Dumbogram.Api.Infrastructure.Files.StorageWriter;
using Dumbogram.Api.Persistence.Context.Application.Entities.Files;
using Dumbogram.Api.Persistence.Context.Application.Enumerations;
using FluentResults;
using MediatR;
using SkiaSharp;
using FileMetadata = Dumbogram.Api.Persistence.Context.Application.Entities.Files.FileMetadata;

namespace Dumbogram.Api.Application.UseCases.FilesGroups.Commands.UploadFilesToFilesGroup;

public class UploadFilesToFilesGroupHandler(
    UserResolverService userResolverService,
    FilesGroupService filesGroupService,
    FileTransferService fileTransferService,
    FileStorageService fileStorageService,
    FileRecordService fileRecordService
) : IRequestHandler<UploadFilesToFilesGroupRequest, Result<Results<string, FileRecord>>>
{
    public async Task<Result<Results<string, FileRecord>>> Handle(
        UploadFilesToFilesGroupRequest request,
        CancellationToken cancellationToken
    )
    {
        var currentUser = await userResolverService.GetApplicationUser();
        var groupId = request.GroupId;

        var groupResult = await filesGroupService.RequestOwnedFilesGroupById(currentUser, groupId);
        if (groupResult.IsFailed) return Result.Fail(groupResult.Errors);
        var group = groupResult.Value;

        var fileContainers = request.FileContainers;

        return await UploadFiles(group, fileContainers);
    }

    private async Task<Results<string, FileRecord>> UploadFiles(
        FilesGroup group,
        IAsyncEnumerable<FileContainer> fileContainers
    )
    {
        var writer = ChooseStorageWriter(group.GroupType);

        var filesQuantityLimit = FilesGroupLimits.GetFilesQuantityLimit(group.GroupType);
        var uploadsLimit = filesQuantityLimit - group.Files.Count();

        var filesResults = await fileTransferService.WriteMultipleFilesAsync(fileContainers, writer, uploadsLimit);

        var uploadedFiles = filesResults.GetSucceededValues().ToList();
        var postprocess = ChoosePostprocess(group.GroupType);

        if (postprocess is not null)
            foreach (var file in uploadedFiles)
                await postprocess(file);

        await fileRecordService.AddFilesRange(uploadedFiles);
        await filesGroupService.AddFilesRangeToFilesGroup(group, uploadedFiles);

        return filesResults;
    }

    private StorageWriter ChooseStorageWriter(FilesGroupType groupType)
    {
        return groupType switch
        {
            FilesGroupType.AttachedPhotos =>
                new StorageWriter()
                    .SetFileFormatValidationPolicy(FileFormatValidationPolicy.ValidateByExtensionAndSignature)
                    .AddPermittedFileFormats(FileFormatGroups.Photo)
                    .SetFileLengthLimit(50_000),

            FilesGroupType.AttachedDocuments =>
                new StorageWriter()
                    .SetFileFormatValidationPolicy(FileFormatValidationPolicy.DoNotValidate),

            _ => throw new SwitchExpressionException()
        };
    }

    private PostprocessFile? ChoosePostprocess(FilesGroupType groupType)
    {
        return groupType switch
        {
            FilesGroupType.AttachedPhotos => PostprocessPhoto,

            // Do nothing by default
            _ => null
        };

        async Task PostprocessPhoto(FileRecord fileRecord)
        {
            await using var imageFile = fileStorageService.ReadFile(fileRecord.StoredFileName);

            using var bitmap = SKBitmap.Decode(imageFile);
            var width = bitmap.Width;
            var height = bitmap.Height;

            fileRecord.Type = FileType.Photo;
            fileRecord.Metadata = FileMetadata.Image(new ImageFileMetadataContent
            {
                Width = width,
                Height = height
            });
        }
    }

    private delegate Task PostprocessFile(FileRecord fileRecord);
}