using Dumbogram.Api.Persistence.Context.Application.Enumerations;

namespace Dumbogram.Api.ApiOld.Files.Services;

public static class FilesGroupLimits
{
    private static readonly Dictionary<FilesGroupType, int> FilesQuantityLimit = new()
    {
        { FilesGroupType.AttachedPhotos, 4 },
        { FilesGroupType.AttachedVideos, 5 },
        { FilesGroupType.AttachedDocuments, 5 }
    };

    public static int GetFilesQuantityLimit(FilesGroupType filesGroupType)
    {
        return FilesQuantityLimit.GetValueOrDefault(filesGroupType, int.MaxValue);
    }
}