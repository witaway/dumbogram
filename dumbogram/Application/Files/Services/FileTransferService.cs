using System.Runtime.CompilerServices;
using Dumbogram.Application.Files.Services.Errors;
using Dumbogram.Application.Files.Services.Exceptions;
using Dumbogram.Application.Files.Services.StorageWriter;
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

    private async Task<Result<File>> WriteFileAsync(
        StorageWriter.StorageWriter writer,
        FileContainerAdapter fileContainer
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

            return new File
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

    private async Task<Result<File>> WriteFileAsync(
        StorageWriter.StorageWriter writer,
        FileMultipartSection fileMultipartSection
    )
    {
        var fileContainer = new FileContainerAdapter(fileMultipartSection);
        return await WriteFileAsync(writer, fileContainer);
    }

    private async Task<Result<File>> WriteFileAsync(
        StorageWriter.StorageWriter writer,
        IFormFile formFile
    )
    {
        var fileContainer = new FileContainerAdapter(formFile);
        return await WriteFileAsync(writer, fileContainer);
    }

    public async Task<List<Result<File>>> UploadMultipleLargeFiles(
        HttpRequest request,
        StorageWriter.StorageWriter writer,
        int uploadsLimit = int.MaxValue
    )
    {
        var uploadedFiles = new List<Result<File>>();
        var successfullyUploadedCount = 0;

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
        
        await foreach (var fileSection in multipartReader.GetFileMultipartSections())
        {
            if (successfullyUploadedCount == uploadsLimit)
            {
                var error = new UploadLimitExceededError();
                uploadedFiles.Add(error);
                continue;
            }

            var writeFileResult = await WriteFileAsync(writer, fileSection);
            if (writeFileResult.IsSuccess) successfullyUploadedCount++;
            uploadedFiles.Add(writeFileResult);
        }

        return uploadedFiles;
    }

    public async Task<Result<File>> UploadSingleLargeFile(
        HttpRequest request,
        StorageWriter.StorageWriter writer
    )
    {
        return (await UploadMultipleLargeFiles(request, writer, 1))
            .First();
    }

    public async Task<List<Result<File>>> UploadMultipleSmallFiles(
        HttpRequest request,
        StorageWriter.StorageWriter writer,
        int uploadsLimit = int.MaxValue
    )
    {
        var uploadedFiles = new List<Result<File>>();
        var successfullyUploadedCount = 0;

        var formFiles = request.Form.Files;
        
        foreach (var formFile in formFiles)
        {
            if (successfullyUploadedCount == uploadsLimit)
            {
                var error = new UploadLimitExceededError();
                uploadedFiles.Add(error);
                continue;
            }

            var writeFileResult = await WriteFileAsync(writer, formFile);
            if (writeFileResult.IsSuccess) successfullyUploadedCount++;
            uploadedFiles.Add(writeFileResult);
        }

        return uploadedFiles;
    }

    public async Task<Result<File>> UploadSingleSmallFile(
        HttpRequest request,
        StorageWriter.StorageWriter writer
    )
    {
        return (await UploadMultipleSmallFiles(request, writer, 1))
            .First();
    }

    public Stream DownloadFile(File file)
    {
        var storedFileName = file.StoredFileName;
        return _fileStorageService.ReadFile(storedFileName);
    }
}