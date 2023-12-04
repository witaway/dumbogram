using Dumbogram.Application.Files.Controllers.Dto;
using Dumbogram.Application.Files.Services;
using Dumbogram.Application.Files.Services.FileFormats;
using Dumbogram.Application.Files.Services.StorageWriter;
using Dumbogram.Infrasctructure.Controller;
using Dumbogram.Infrasctructure.Filters;
using Dumbogram.Models.Files;
using Microsoft.AspNetCore.Mvc;
using SkiaSharp;
using File = Dumbogram.Models.Files.File;

namespace Dumbogram.Application.Files.Controllers;

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

        var filesResults = await _fileTransferService.UploadSmallFiles<FilePhoto>(Request, writer, 3);

        var uploadedFiles = filesResults.GetSucceededValues().ToList();

        foreach (var file in uploadedFiles)
        {
            await using var imageFile = _fileStorageService.ReadFile(file.StoredFileName);

            using var bitmap = SKBitmap.Decode(imageFile);
            var width = bitmap.Width;
            var height = bitmap.Height;

            file.Metadata.Width = width;
            file.Metadata.Height = height;
        }

        await _fileService.AddFilesRange(uploadedFiles);

        var response = FilesUploadResponse.Parse(filesResults);
        return Created("", response);
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

        var filesResults = await _fileTransferService.UploadLargeFiles<File>(Request, writer);

        var uploadedFiles = filesResults.GetSucceededValues();
        await _fileService.AddFilesRange(uploadedFiles);

        var response = FilesUploadResponse.Parse(filesResults);
        return Created("", response);
    }
}