using Dumbogram.Infrasctructure.Classes;
using File = Dumbogram.Models.Files.File;

namespace Dumbogram.Application.Files.Controllers.Dto;

public class FilesUploadResponse : List<UploadResultDto>
{
    public FilesUploadResponse(Results<string, File> filesUploadResults)
    {
        var allResults = filesUploadResults.GetAllResultsWithIdentity();
        var uploadResultDtos = allResults.Select(x => new UploadResultDto(x));
        AddRange(uploadResultDtos);
    }

    public FilesUploadResponse(IEnumerable<UploadResultDto> uploadResultDtos)
    {
        AddRange(uploadResultDtos);
    }
}