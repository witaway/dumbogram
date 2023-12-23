using Dumbogram.Api.Persistence.Context.Application.Entities.Files;
using Dumbogram.Api.Persistence.Context.Application.Enumerations;
using FluentResults;
using MediatR;

namespace Dumbogram.Api.Application.UseCases.FilesGroups.Commands.CreateFilesGroup;

public record CreateFilesGroupRequest(string FilesGroupTypeName) : IRequest<Result<FilesGroup>>
{
    public FilesGroupType FilesGroupType => FilesGroupTypeName.Trim().ToLower() switch
    {
        "photos" => FilesGroupType.AttachedPhotos,
        "videos" => FilesGroupType.AttachedVideos,
        "documents" => FilesGroupType.AttachedDocuments,
        _ => throw new BadHttpRequestException("Incorrect group type")
    };
}