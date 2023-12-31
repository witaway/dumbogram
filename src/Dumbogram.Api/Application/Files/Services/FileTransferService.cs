﻿using System.Runtime.CompilerServices;
using Dumbogram.Api.Application.Files.Services.Errors;
using Dumbogram.Api.Application.Files.Services.Exceptions;
using Dumbogram.Api.Application.Files.Services.StorageWriter;
using Dumbogram.Api.Infrasctructure.Classes;
using Dumbogram.Api.Infrasctructure.Errors;
using Dumbogram.Api.Infrasctructure.Extensions;
using Dumbogram.Api.Infrasctructure.Utilities;
using Dumbogram.Api.Persistence.Context.Application.Entities.Files;
using FluentResults;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;

namespace Dumbogram.Api.Application.Files.Services;

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

    private async Task<Result<FileRecord>> WriteSingleFileAsync(
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

    private async Task<Results<string, FileRecord>> WriteMultipleFilesAsync(
        StorageWriter.StorageWriter writer,
        IAsyncEnumerable<FileContainer> fileContainers,
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

    public async Task<Results<string, FileRecord>> UploadLargeFiles(
        HttpRequest request,
        StorageWriter.StorageWriter writer,
        int uploadsLimit = int.MaxValue
    )
    {
        var contentType = request.ContentType ?? "";

        if (!MultipartRequestHelper.IsMultipartContentType(contentType))
            throw new Exception("The request couldn't be processed");

        var boundary = MultipartRequestHelper.GetBoundary(
            MediaTypeHeaderValue.Parse(contentType),
            DefaultFormOptions.MultipartBoundaryLengthLimit
        );

        var multipartReader = new MultipartReader(boundary, request.Body);

        var fileContainers = multipartReader.GetFileContainers();
        var uploadedFilesResults = await WriteMultipleFilesAsync(writer, fileContainers, uploadsLimit);

        return uploadedFilesResults;
    }

    public async Task<Results<string, FileRecord>> UploadSmallFiles(
        HttpRequest request,
        StorageWriter.StorageWriter writer,
        int uploadsLimit = int.MaxValue
    )
    {
        var formFiles = request.Form.Files;

        var fileContainers = formFiles.GetFileContainers();
        var uploadedFilesResults = await WriteMultipleFilesAsync(writer, fileContainers, uploadsLimit);

        return uploadedFilesResults;
    }

    public Stream DownloadFile(FileRecord fileRecord)
    {
        var storedFileName = fileRecord.StoredFileName;
        return _fileStorageService.ReadFile(storedFileName);
    }
}