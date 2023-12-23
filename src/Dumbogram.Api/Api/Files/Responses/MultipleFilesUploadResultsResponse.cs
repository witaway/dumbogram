using Dumbogram.Api.Common.Classes;
using Dumbogram.Api.Persistence.Context.Application.Entities.Files;
using FluentResults;

namespace Dumbogram.Api.Api.Files.Responses;

public class MultipleFilesUploadResultsResponse : List<SingleFileUploadResultResponse>
{
    public MultipleFilesUploadResultsResponse(Results<string, FileRecord> filesUploadResults)
    {
        var allResults = filesUploadResults.GetAllResultsWithIdentity();
        var uploadResultDto = allResults.Select(x => new SingleFileUploadResultResponse(x));
        AddRange(uploadResultDto);
    }

    public MultipleFilesUploadResultsResponse(IEnumerable<SingleFileUploadResultResponse> uploadResultDtos)
    {
        AddRange(uploadResultDtos);
    }

    [Obsolete("Semi incorrect shit. I hope will be deleted soon")]
    public static MultipleFilesUploadResultsResponse Parse(Results<string, FileRecord> filesUploadResults)
    {
        var newUploadResults = new Results<string, FileRecord>();
        foreach (var identityWithResult in filesUploadResults.GetAllResultsWithIdentity())
        {
            var result = identityWithResult.Result.IsSuccess
                ? Result.Ok(identityWithResult.Result.Value)
                : Result.Fail(identityWithResult.Result.Errors);

            newUploadResults.Add(identityWithResult.Identity.Label, result);
        }

        return new MultipleFilesUploadResultsResponse(newUploadResults);
    }
}