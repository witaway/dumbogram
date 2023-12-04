using Dumbogram.Infrasctructure.Classes;
using FluentResults;
using File = Dumbogram.Models.Files.File;

namespace Dumbogram.Application.Files.Controllers.Dto;

public class FilesUploadResponse : List<UploadResultDto>
{
    private FilesUploadResponse(Results<string, File> filesUploadResults)
    {
        var allResults = filesUploadResults.GetAllResultsWithIdentity();
        var uploadResultDtos = allResults.Select(x => new UploadResultDto(x));
        AddRange(uploadResultDtos);
    }

    private FilesUploadResponse(IEnumerable<UploadResultDto> uploadResultDtos)
    {
        AddRange(uploadResultDtos);
    }

    public static FilesUploadResponse Parse<TFile>(Results<string, TFile> filesUploadResults) where TFile : File
    {
        // TODO: THIS IS A COMPLETE SHIT. FIX IT! DO NOT COPY ALL THIS! SHIT SHIT SHIT
        var newUploadResults = new Results<string, File>();
        foreach (var identityWithResult in filesUploadResults.GetAllResultsWithIdentity())
        {
            var result = identityWithResult.Result.IsSuccess
                ? Result.Ok((File)identityWithResult.Result.Value)
                : Result.Fail(identityWithResult.Result.Errors);

            newUploadResults.Add(identityWithResult.Identity.Label, result);
        }

        return new FilesUploadResponse(newUploadResults);
    }
}