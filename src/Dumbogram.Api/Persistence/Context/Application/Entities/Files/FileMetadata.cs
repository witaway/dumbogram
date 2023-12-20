namespace Dumbogram.Api.Persistence.Context.Application.Entities.Files;

public sealed class FileMetadata
{
    public int? Width { get; init; }
    public int? Height { get; init; }
    public int? Duration { get; init; }
    public bool? IsExecutable { get; init; }

    public static FileMetadata Image(ImageFileMetadataContent content)
    {
        return new FileMetadata
        {
            Width = content.Width,
            Height = content.Height
        };
    }

    public static FileMetadata Video(VideoFileMetadataContent content)
    {
        return new FileMetadata
        {
            Width = content.Width,
            Height = content.Height,
            Duration = content.Duration
        };
    }

    public static FileMetadata Animation(AnimationFileMetadataContent content)
    {
        return new FileMetadata
        {
            Width = content.Width,
            Height = content.Height,
            Duration = content.Duration
        };
    }

    public static FileMetadata Document(DocumentFileMetadataContent content)
    {
        return new FileMetadata
        {
            IsExecutable = content.IsExecutable
        };
    }
}

public sealed class ImageFileMetadataContent
{
    public int? Width { get; init; }
    public int? Height { get; init; }
}

public sealed class VideoFileMetadataContent
{
    public int? Width { get; init; }
    public int? Height { get; init; }
    public int? Duration { get; init; }
}

public sealed class AnimationFileMetadataContent
{
    public int? Width { get; init; }
    public int? Height { get; init; }
    public int? Duration { get; init; }
}

public sealed class DocumentFileMetadataContent
{
    public bool? IsExecutable { get; init; }
}