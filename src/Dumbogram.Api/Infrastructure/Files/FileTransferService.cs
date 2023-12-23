using System.Runtime.CompilerServices;
using Dumbogram.Api.Common.Classes;
using Dumbogram.Api.Common.Errors;
using Dumbogram.Api.Infrastructure.Files.Errors;
using Dumbogram.Api.Infrastructure.Files.Exceptions;
using Dumbogram.Api.Infrastructure.Files.StorageWriter;
using Dumbogram.Api.Persistence.Context.Application.Entities.Files;
using FluentResults;

namespace Dumbogram.Api.Infrastructure.Files;

public class FileTransferService
{
    private readonly FileStorageService _fileStorageService;

    public FileTransferService(
        FileStorageService fileStorageService
    )
    {
        _fileStorageService = fileStorageService;
    }

    public async Task<Result<FileRecord>> WriteSingleFileAsync(
        StorageWriter.StorageWriter writer,
        FileContainer fileContainer
    )
    {
        var fileMetadata = fileContainer.FileMetadata;
        await using var destination = _fileStorageService.CreateFile(
            out var filePath,
            fileMetadata.Extension
        );

        try
        {
            await writer.Write(fileContainer, destination);
            var savedFileInfo = _fileStorageService.GetFileInfo(filePath);

            return new FileRecord
            {
                StoredFileName = filePath,
                OriginalFileName = fileMetadata.TrustedFileNameForDisplay,
                MimeType = fileMetadata.MimeType,
                FileSize = savedFileInfo.Length
            };
        }
        catch (FileUploadException exception)
        {
            _fileStorageService.DeleteFile(filePath);
            ApplicationApiError error = exception switch
            {
                FileTooBigException => new FileTooBigError(),
                FileTypeIncorrectException => new FileTypeIncorrectError(),
                _ => throw new SwitchExpressionException()
            };
            return Result.Fail(error);
        }
        catch (Exception exception)
        {
            _fileStorageService.DeleteFile(filePath);
            throw;
        }
    }

    public async Task<Results<string, FileRecord>> WriteMultipleFilesAsync(
        IAsyncEnumerable<FileContainer> fileContainers,
        StorageWriter.StorageWriter writer,
        int uploadsLimit = int.MaxValue
    )
    {
        var uploadedFiles = new Results<string, FileRecord>();
        var successfullyUploadedCount = 0;

        foreach (var fileContainer in fileContainers.ToBlockingEnumerable())
        {
            var fileName = fileContainer.Filename;

            if (successfullyUploadedCount == uploadsLimit)
            {
                var error = new UploadLimitExceededError();
                uploadedFiles.Add(fileName, error);
                continue;
            }

            var writeFileResult = await WriteSingleFileAsync(writer, fileContainer);
            if (writeFileResult.IsSuccess) successfullyUploadedCount++;
            uploadedFiles.Add(fileName, writeFileResult);
        }

        return uploadedFiles;
    }

    public Stream DownloadFile(FileRecord fileRecord)
    {
        var storedFileName = fileRecord.StoredFileName;
        return _fileStorageService.ReadFile(storedFileName);
    }
}