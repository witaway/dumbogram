namespace Dumbogram.Api.Application.UseCases.FilesGroups.Queries.DownloadFileFromFilesGroup;

public record DownloadFileFromFilesGroupResult(Stream FileStream, string? DownloadName, string ContentType);