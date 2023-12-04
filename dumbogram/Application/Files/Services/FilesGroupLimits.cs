﻿namespace Dumbogram.Models.Files;

public static class FilesGroupLimits
{
    private static readonly Dictionary<FilesGroupType, int> FilesQuantityLimit = new()
    {
        { FilesGroupType.AttachedPhotos, 10 },
        { FilesGroupType.AttachedVideos, 5 },
        { FilesGroupType.AttachedDocuments, 5 }
    };

    public static int GetFilesQuantityLimit(FilesGroupType filesGroupType)
    {
        return FilesQuantityLimit.GetValueOrDefault(filesGroupType, int.MaxValue);
    }
}