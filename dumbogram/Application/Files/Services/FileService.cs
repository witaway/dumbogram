using Dumbogram.Application.Files.Services.Errors;
using Dumbogram.Database;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using File = Dumbogram.Models.Files.File;

namespace Dumbogram.Application.Files.Services;

public class FileService
{
    private readonly ApplicationDbContext _dbContext;

    public FileService(
        ApplicationDbContext dbContext
    )
    {
        _dbContext = dbContext;
    }

    public async Task AddFile(File file)
    {
        await _dbContext.Files.AddAsync(file);
        await _dbContext.SaveChangesAsync();
    }

    public async Task AddFilesRange(IEnumerable<File> files)
    {
        await _dbContext.Files.AddRangeAsync(files);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteFile(File file)
    {
        _dbContext.Remove(file);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<File?> GetFileById(Guid fileId)
    {
        var query = _dbContext
            .Files
            .Include(file => file.FilesGroup)
            .Where(file => file.Id == fileId);

        return await query.SingleOrDefaultAsync();
    }

    public async Task<Result<File>> RequestFileById(Guid fileId)
    {
        var file = await GetFileById(fileId);
        if (file == null)
        {
            return Result.Fail(new FileNotExistError());
        }

        return Result.Ok(file);
    }
}