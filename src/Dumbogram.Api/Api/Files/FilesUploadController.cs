using Dumbogram.Api.Api.Files.Responses;
using Dumbogram.Api.Application.UseCases.FilesGroups.Commands.CreateFilesGroup;
using Dumbogram.Api.Application.UseCases.FilesGroups.Commands.DeleteFileFromFilesGroup;
using Dumbogram.Api.Application.UseCases.FilesGroups.Commands.UploadFilesToFilesGroup;
using Dumbogram.Api.Application.UseCases.FilesGroups.Queries.DownloadFileFromFilesGroup;
using Dumbogram.Api.Application.UseCases.FilesGroups.Queries.GetFilesGroup;
using Dumbogram.Api.Common.Controller;
using Dumbogram.Api.Common.Filters;
using Dumbogram.Api.Infrastructure.Files;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dumbogram.Api.Api.Files;

[Authorize]
[Route("api/files")]
public class FilesUploadController(IMediator mediator) : ApplicationController
{
    [HttpPost("groups")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    [MultipartFormData]
    [DisableRequestSizeLimit]
    [DisableFormValueModelBinding]
    public async Task<IActionResult> CreateGroup([FromQuery(Name = "group_type")] string groupTypeName)
    {
        var createFilesGroupRequest = new CreateFilesGroupRequest(groupTypeName);
        var createFilesGroupResult = await mediator.Send(createFilesGroupRequest);

        if (createFilesGroupResult.IsFailed) return Failure(createFilesGroupResult.Errors);
        var filesGroup = createFilesGroupResult.Value;
        var filesGroupId = filesGroup.Id;

        // After files group is created, try to upload files to it
        return await UploadFilesToGroup(filesGroupId);
    }

    [HttpGet("groups/{groupId:guid}", Name = nameof(GetFilesGroup))]
    public async Task<IActionResult> GetFilesGroup(Guid groupId)
    {
        var request = new GetFilesGroupRequest(groupId);
        var result = await mediator.Send(request);

        if (result.IsFailed) return Failure(result.Errors);
        var filesGroup = result.Value;

        var filesGroupDto = new SingleFilesGroupResponse(filesGroup);
        return Ok(filesGroupDto);
    }

    [HttpDelete("groups/{groupId}/{fileId}")]
    public async Task<IActionResult> RemoveFileFromGroup(Guid groupId, Guid fileId)
    {
        var request = new DeleteFileFromFilesGroupRequest(groupId, fileId);
        var result = await mediator.Send(request);

        if (result.IsFailed) return Failure(result.Errors);

        return NoContent();
    }

    [HttpPost("groups/{groupId:guid}", Name = nameof(UploadFilesToGroup))]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
    [MultipartFormData]
    [DisableRequestSizeLimit]
    [DisableFormValueModelBinding]
    public async Task<IActionResult> UploadFilesToGroup(Guid groupId)
    {
        var fileContainers = Request.GetFileContainersFromMultipart();

        // Retrieve files group (needed to make correct response)
        var getFilesGroupRequest = new GetFilesGroupRequest(groupId);
        var getFilesGroupResult = await mediator.Send(getFilesGroupRequest);
        if (getFilesGroupResult.IsFailed) return Failure(getFilesGroupResult.Errors);

        // Upload files into this group
        var uploadFilesToGroupRequest = new UploadFilesToFilesGroupRequest(groupId, fileContainers);
        var uploadFilesToGroupResult = await mediator.Send(uploadFilesToGroupRequest);
        if (uploadFilesToGroupResult.IsFailed) return Failure(uploadFilesToGroupResult.Errors);

        // Send response
        var filesGroup = getFilesGroupResult.Value;
        var resultsPerFile = uploadFilesToGroupResult.Value;

        var resultsPerFileDto = new MultipleFilesUploadResultsResponse(resultsPerFile);
        var uploadDto = new SingleFilesGroupWithUploadResultResponse(filesGroup, resultsPerFileDto);

        return Created("", uploadDto);
    }

    [HttpGet("groups/{groupId:guid}/{fileId:guid}")]
    public async Task<IActionResult> DownloadFileFromGroup(Guid groupId, Guid fileId)
    {
        var request = new DownloadFileFromFilesGroupRequest(groupId, fileId);
        var result = await mediator.Send(request);

        if (result.IsFailed) return Failure(result.Errors);
        var downloadInfo = result.Value;

        // fileStream will be disposed automatically when response become fully send
        var (fileStream, downloadName, contentType) = result.Value;

        return File(fileStream, contentType, downloadName);
    }
}