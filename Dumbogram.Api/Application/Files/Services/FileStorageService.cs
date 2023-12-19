using Microsoft.VisualBasic.FileIO;

namespace Dumbogram.Api.Application.Files.Services;

public class FileStorageService
{
    private readonly string _storagePath;

    public FileStorageService()
    {
        // Todo: Add configuration through appsettings.json
        _storagePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "FileStorage"));
        FileSystem.CreateDirectory(_storagePath);
    }

    public FileStream CreateFile(out string relativeFilePath, string? extension = null)
    {
        relativeFilePath = GenerateRelativeFilePath(extension);
        var absoluteFilePath = GetFullFilePath(relativeFilePath);

        return new FileStream(
            absoluteFilePath,
            FileMode.Create,
            FileAccess.Write,
            FileShare.Read,
            1024
        );
    }

    public FileStream ReadFile(string relativeFilePath)
    {
        var absoluteFilePath = GetFullFilePath(relativeFilePath);

        return new FileStream(
            absoluteFilePath,
            FileMode.Open,
            FileAccess.Read,
            FileShare.Read,
            1024
        );
    }

    public void DeleteFile(string relativeFilePath)
    {
        var absoluteFilePath = GetFullFilePath(relativeFilePath);
        FileSystem.DeleteFile(absoluteFilePath);
    }

    public FileInfo GetFileInfo(string relativeFilePath)
    {
        var absoluteFilePath = GetFullFilePath(relativeFilePath);
        return FileSystem.GetFileInfo(absoluteFilePath);
    }

    private string GenerateRelativeFilePath(string? extension)
    {
        var filename = Guid.NewGuid().ToString();
        if (extension != null)
        {
            filename = Path.ChangeExtension(filename, extension);
        }

        return filename;
    }

    private string GetFullFilePath(string relativeFilePath)
    {
        var fullPath = Path.GetFullPath(Path.Combine(_storagePath, relativeFilePath));
        if (fullPath.StartsWith(_storagePath))
        {
            return fullPath;
        }

        throw new Exception("Incorrect path");
    }
}