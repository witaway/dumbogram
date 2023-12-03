using Dumbogram.Application.Files.Services;
using Dumbogram.Application.Files.Services.FileFormats;
using Dumbogram.Application.Files.Services.StorageWriter;
using Dumbogram.Infrasctructure.Controller;
using Dumbogram.Infrasctructure.Filters;
using Dumbogram.Models.Files;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using SkiaSharp;

namespace Dumbogram.Application.Files.Controllers;

public class UploadSingleFileRequest
{
    public IFormFile File;
}

[Route("api/files")]
public class FileController : ApplicationController
{
    private readonly FileService _fileService;
    private readonly FilesGroupService _filesGroupService;
    private readonly FileStorageService _fileStorageService;
    private readonly FileTransferService _fileTransferService;

    public FileController(
        FileTransferService fileTransferService,
        FileService fileService,
        FilesGroupService filesGroupService,
        FileStorageService fileStorageService
    )
    {
        _fileTransferService = fileTransferService;
        _fileService = fileService;
        _filesGroupService = filesGroupService;
        _fileStorageService = fileStorageService;
    }

    [HttpGet("{fileId:guid}")]
    public async Task<IActionResult> DownloadFile(Guid fileId)
    {
        var fileResult = await _fileService.RequestFileById(fileId);
        if (fileResult.IsFailed)
        {
            return Failure(fileResult.Errors);
        }

        var file = fileResult.Value;

        var contentType = file.MimeType;
        var downloadName = file.OriginalFileName;
        var fileStream = _fileTransferService.DownloadFile(file);
        
        return File(fileStream, contentType, downloadName);
    }

    [HttpPost("photo")]
    public async Task<IActionResult> UploadPhoto()
    {
        var writer = new StorageWriter()
            .MatchPolicy(FileFormatValidationPolicy.ValidateByExtensionAndSignature)
            .AddFileFormats(FileFormatGroups.Photo);

        var formFile = Request.Form.Files.First();
        var file = await _fileTransferService.UploadSingleSmallFile(formFile, writer);

        if (file == null)
        {
            return Failure(new Error("Error during download"));
        }

        await using var imageFile = _fileStorageService.ReadFile(file.StoredFileName);

        var bitmap = SKBitmap.Decode(imageFile);
        var width = bitmap.Width;
        var height = bitmap.Height;

        var filePhoto = new FilePhoto(file)
        {
            Width = width,
            Height = height
        };

        await _fileService.AddFile(filePhoto);

        var fileUri = $"/api/files/{filePhoto.Id}";
        return Created(fileUri, null);
    }

    [HttpPost("document")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    [MultipartFormData]
    [DisableFormValueModelBinding]
    public async Task<IActionResult> Upload()
    {
        var writer = new StorageWriter()
            .MatchPolicy(FileFormatValidationPolicy.DoNotValidate);

        var files = await _fileTransferService.UploadMultipleLargeFiles(Request, writer);

        await _fileService.AddFilesRange(files);

        var fileUri = "/api/files/bzzzzzzzzzzzzzz";
        return Created(fileUri, null);
    }
}