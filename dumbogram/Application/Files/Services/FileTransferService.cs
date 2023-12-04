using System.Runtime.CompilerServices;
using Dumbogram.Application.Files.Services.Errors;
using Dumbogram.Application.Files.Services.Exceptions;
using Dumbogram.Application.Files.Services.StorageWriter;
using Dumbogram.Infrasctructure.Classes;
using Dumbogram.Infrasctructure.Errors;
using Dumbogram.Infrasctructure.Extensions;
using Dumbogram.Infrasctructure.Utilities;
using FluentResults;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using File = Dumbogram.Models.Files.File;

namespace Dumbogram.Application.Files.Services;

public class FileTransferService
{
    // Get the default form options so that we can use them to set the default 
    // limits for request body data.
    private static readonly FormOptions DefaultFormOptions = new();

    private readonly FileStorageService _fileStorageService;

    public FileTransferService(
        FileStorageService fileStorageService
    )
    {
        _fileStorageService = fileStorageService;
    }

    private async Task<Result<TFile>> WriteSingleFileAsync<TFile>(
        StorageWriter.StorageWriter writer,
        FileContainer fileContainer
    ) where TFile : File, new()
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

            return new TFile
            {
                StoredFileName = filePath,
                OriginalFileName = fileMetadata.TrustedFileNameForDisplay,
                MimeType = fileMetadata.MimeType,
                FileSize = savedFileInfo.Length
            };
        }
        catch (FileUploadException exception)
        {
            await destination.DisposeAsync();
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
            await destination.DisposeAsync();
            _fileStorageService.DeleteFile(filePath);
            throw;
        }
    }

    private async Task<Results<string, TFile>> WriteMultipleFilesAsync<TFile>(
        StorageWriter.StorageWriter writer,
        IAsyncEnumerable<FileContainer> fileContainers,
        int uploadsLimit = int.MaxValue
    ) where TFile : File, new()
    {
        var uploadedFiles = new Results<string, TFile>();
        var successfullyUploadedCount = 0;

        await foreach (var fileContainer in fileContainers)
        {
            var fileName = fileContainer.Filename;

            if (successfullyUploadedCount == uploadsLimit)
            {
                var error = new UploadLimitExceededError();
                uploadedFiles.Add(fileName, error);
                continue;
            }

            var writeFileResult = await WriteSingleFileAsync<TFile>(writer, fileContainer);
            if (writeFileResult.IsSuccess) successfullyUploadedCount++;
            uploadedFiles.Add(fileName, writeFileResult);
        }

        return uploadedFiles;
    }

    public async Task<Results<string, TFile>> UploadLargeFiles<TFile>(
        HttpRequest request,
        StorageWriter.StorageWriter writer,
        int uploadsLimit = int.MaxValue
    ) where TFile : File, new()
    {
        var contentType = request.ContentType ?? "";

        if (!MultipartRequestHelper.IsMultipartContentType(contentType))
        {
            throw new Exception("The request couldn't be processed");
        }

        var boundary = MultipartRequestHelper.GetBoundary(
            MediaTypeHeaderValue.Parse(contentType),
            DefaultFormOptions.MultipartBoundaryLengthLimit
        );

        var multipartReader = new MultipartReader(boundary, request.Body);

        var fileContainers = multipartReader.GetFileContainers();
        var uploadedFilesResults = await WriteMultipleFilesAsync<TFile>(writer, fileContainers, uploadsLimit);

        return uploadedFilesResults;
    }

    public async Task<Results<string, TFile>> UploadSmallFiles<TFile>(
        HttpRequest request,
        StorageWriter.StorageWriter writer,
        int uploadsLimit = int.MaxValue
    ) where TFile : File, new()
    {
        var formFiles = request.Form.Files;

        var fileContainers = formFiles.GetFileContainers();
        var uploadedFilesResults = await WriteMultipleFilesAsync<TFile>(writer, fileContainers, uploadsLimit);

        return uploadedFilesResults;
    }

    public Stream DownloadFile(File file)
    {
        var storedFileName = file.StoredFileName;
        return _fileStorageService.ReadFile(storedFileName);
    }
}