using Dumbogram.Infrasctructure.Classes;
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
        return new FilesUploadResponse((Results<string, File>)(object)filesUploadResults);
    }
}