using Dumbogram.Application.Files.Controllers.Dto;
using Dumbogram.Application.Files.Services;
using Dumbogram.Application.Files.Services.FileFormats;
using Dumbogram.Application.Files.Services.StorageWriter;
using Dumbogram.Infrasctructure.Controller;
using Dumbogram.Infrasctructure.Filters;
using Dumbogram.Models.Files;
using Microsoft.AspNetCore.Mvc;
using SkiaSharp;

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

        var filesResults = await _fileTransferService.UploadSmallFiles(Request, writer, 3);

        var uploadedFiles = filesResults.GetSucceededValues();
        var uploadedPhotoFiles = new List<FilePhoto>();

        foreach (var file in uploadedFiles)
        {
            await using var imageFile = _fileStorageService.ReadFile(file.StoredFileName);

            using var bitmap = SKBitmap.Decode(imageFile);
            var width = bitmap.Width;
            var height = bitmap.Height;

            var filePhoto = new FilePhoto(file, new FilePhotoMetadata
            {
                Width = width,
                Height = height
            });

            uploadedPhotoFiles.Add(filePhoto);
        }

        await _fileService.AddFilesRange(uploadedPhotoFiles);

        var response = new FilesUploadResponse(filesResults);
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

        var filesResults = await _fileTransferService.UploadLargeFiles(Request, writer);

        var uploadedFiles = filesResults.GetSucceededValues();
        await _fileService.AddFilesRange(uploadedFiles);

        var response = new FilesUploadResponse(filesResults);
        return Created("", response);
    }
}