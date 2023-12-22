using Dumbogram.Api.Infrasctructure.Classes;
using Dumbogram.Api.Persistence.Context.Application.Entities.Files;
using FluentResults;

namespace Dumbogram.Api.ApiOld.Files.Controllers.Dto;

public class FilesUploadResponse : List<UploadResultDto>
{
    private FilesUploadResponse(Results<string, FileRecord> filesUploadResults)
    {
        var allResults = filesUploadResults.GetAllResultsWithIdentity();
        var uploadResultDtos = allResults.Select(x => new UploadResultDto(x));
        AddRange(uploadResultDtos);
    }

    private FilesUploadResponse(IEnumerable<UploadResultDto> uploadResultDtos)
    {
        AddRange(uploadResultDtos);
    }

    public static FilesUploadResponse Parse<TFile>(Results<string, TFile> filesUploadResults) where TFile : FileRecord
    {
        // TODO: THIS IS A COMPLETE SHIT. FIX IT! DO NOT COPY ALL THIS! SHIT SHIT SHIT
        var newUploadResults = new Results<string, FileRecord>();
        foreach (var identityWithResult in filesUploadResults.GetAllResultsWithIdentity())
        {
            var result = identityWithResult.Result.IsSuccess
                ? Result.Ok((FileRecord)identityWithResult.Result.Value)
                : Result.Fail(identityWithResult.Result.Errors);

            newUploadResults.Add(identityWithResult.Identity.Label, result);
        }

        return new FilesUploadResponse(newUploadResults);
    }
}