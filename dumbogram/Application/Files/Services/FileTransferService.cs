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

    public async Task<List<File>> UploadMultipleLargeFiles(
        HttpRequest request,
        StorageWriter.StorageWriter writer
    )
    {
        var uploadedFiles = new List<File>();

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
            var writeFileResult = await WriteFileAsync(writer, fileSection);
            if (writeFileResult.IsSuccess)
            {
                var file = writeFileResult.Value;
                uploadedFiles.Add(file);
            }
        }

        return uploadedFiles;
    }

    public async Task<List<File>> UploadMultipleSmallFiles(List<IFormFile> formFiles,
        StorageWriter.StorageWriter writer)
    {
        var uploadedFiles = new List<File>();

        foreach (var formFile in formFiles)
        {
            var writeFileResult = await WriteFileAsync(writer, formFile);
            if (writeFileResult.IsSuccess)
            {
                var file = writeFileResult.Value;
                uploadedFiles.Add(file);
            }
        }

        return uploadedFiles;
    }

    public async Task<File?> UploadSingleSmallFile(IFormFile formFile, StorageWriter.StorageWriter writer)
    {
        var uploaded = await UploadMultipleSmallFiles(
            new List<IFormFile> { formFile },
            writer
        );

        return uploaded.FirstOrDefault();
    }

    public Stream DownloadFile(File file)
    {
        var storedFileName = file.StoredFileName;
        return _fileStorageService.ReadFile(storedFileName);
    }
}