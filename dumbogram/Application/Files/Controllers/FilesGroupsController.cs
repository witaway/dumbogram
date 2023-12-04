using Dumbogram.Application.Files.Controllers.Dto;
using Dumbogram.Application.Files.Services;
using Dumbogram.Application.Files.Services.Errors;
using Dumbogram.Application.Users.Services;
using Dumbogram.Infrasctructure.Controller;
using Dumbogram.Infrasctructure.Filters;
using Dumbogram.Models.Files;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dumbogram.Application.Files.Controllers;

[Authorize]
[Route("api/files")]
public class FileController : ApplicationController
{
    private readonly FileService _fileService;
    private readonly FilesGroupService _filesGroupService;
    private readonly FileStorageService _fileStorageService;
    private readonly FileTransferService _fileTransferService;
    private readonly UploadService _uploadService;
    private readonly UserResolverService _userResolverService;

    public FileController(
        FileTransferService fileTransferService,
        FileService fileService,
        UploadService uploadService,
        FilesGroupService filesGroupService,
        FileStorageService fileStorageService,
        UserResolverService userResolverService
    )
    {
        _fileTransferService = fileTransferService;
        _fileService = fileService;
        _uploadService = uploadService;
        _filesGroupService = filesGroupService;
        _fileStorageService = fileStorageService;
        _userResolverService = userResolverService;
    }

    [HttpPost("groups")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    [MultipartFormData]
    [DisableFormValueModelBinding]
    public async Task<IActionResult> CreateGroup([FromQuery(Name = "group_type")] string groupTypeName)
    {
        var subjectUser = await _userResolverService.GetApplicationUser();
        var groupType = groupTypeName switch
        {
            "photos" => FilesGroupType.AttachedPhotos,
            "videos" => FilesGroupType.AttachedVideos,
            "documents" => FilesGroupType.AttachedDocuments,
            _ => throw new BadHttpRequestException("Incorrect group type")
        };

        var group = await _filesGroupService.CreateFilesGroup(subjectUser, groupType);

        FilesUploadResponse? uploadDto = null;

        if (Request.Form.Files.Count > 0)
        {
            uploadDto = await _uploadService.UploadIntoGroup(group);
        }

        var response = new CreateSingleGroupRequest(group, uploadDto);
        return Created("", response);
    }

    [HttpGet("groups/{groupId:guid}")]
    public async Task<IActionResult> GetGroup(Guid groupId)
    {
        var subjectUser = await _userResolverService.GetApplicationUser();

        var groupResult = await _filesGroupService.RequestFilesGroupById(groupId);
        if (groupResult.IsFailed)
        {
            return Failure(groupResult.Errors);
        }

        var group = groupResult.Value;
        var response = new GetSingleGroupRequest(group);

        return Ok(response);
    }

    [HttpPost("groups/{groupId:guid}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    [MultipartFormData]
    [DisableFormValueModelBinding]
    public async Task<IActionResult> UploadFilesToGroup(Guid groupId)
    {
        var subjectUser = await _userResolverService.GetApplicationUser();

        var groupResult = await _filesGroupService.RequestOwnedFilesGroupById(subjectUser, groupId);
        if (groupResult.IsFailed)
        {
            return Failure(groupResult.Errors);
        }

        var group = groupResult.Value;

        var uploadDto = await _uploadService.UploadIntoGroup(group);

        var response = new CreateSingleGroupRequest(group, uploadDto);
        return Created("", response);
    }

    [HttpGet("groups/{groupId:guid}/{fileId:guid}")]
    public async Task<IActionResult> DownloadFileFromGroup(Guid groupId, Guid fileId)
    {
        var groupResult = await _filesGroupService.RequestFilesGroupById(groupId);
        if (groupResult.IsFailed)
        {
            return Failure(groupResult.Errors);
        }

        var group = groupResult.Value;
        var file = group.Files.SingleOrDefault(file => file.Id == fileId);

        if (file == null)
        {
            return Failure(new FileNotExistError());
        }

        var contentType = file.MimeType;
        var downloadName = file.OriginalFileName;
        var fileStream = _fileTransferService.DownloadFile(file);

        return File(fileStream, contentType, downloadName);
    }

    [HttpDelete("groups/{groupId}/{fileId}")]
    public async Task<IActionResult> RemoveFileFromGroup(Guid groupId, Guid fileId)
    {
        var groupResult = await _filesGroupService.RequestFilesGroupById(groupId);
        if (groupResult.IsFailed)
        {
            return Failure(groupResult.Errors);
        }

        var group = groupResult.Value;
        var file = group.Files.SingleOrDefault(file => file.Id == fileId);

        if (file == null)
        {
            return Failure(new FileNotExistError());
        }

        await _filesGroupService.RemoveFileFromFilesGroup(group, file);

        return Ok();
    }
}