using Dumbogram.Api.ApiOld.Files.Services.Errors;
using Dumbogram.Api.Persistence.Context.Application;
using Dumbogram.Api.Persistence.Context.Application.Entities.Files;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Dumbogram.Api.ApiOld.Files.Services;

public class FileRecordService
{
    private readonly ApplicationDbContext _dbContext;

    public FileRecordService(
        ApplicationDbContext dbContext
    )
    {
        _dbContext = dbContext;
    }

    public async Task AddFile(FileRecord fileRecord)
    {
        await _dbContext.FilesRecords.AddAsync(fileRecord);
        await _dbContext.SaveChangesAsync();
    }

    public async Task AddFilesRange(IEnumerable<FileRecord> files)
    {
        await _dbContext.FilesRecords.AddRangeAsync(files);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteFile(FileRecord fileRecord)
    {
        _dbContext.Remove(fileRecord);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<FileRecord?> GetFileById(Guid fileId)
    {
        var query = _dbContext
            .FilesRecords
            .Include(file => file.FilesGroup)
            .Where(file => file.Id == fileId);

        return await query.SingleOrDefaultAsync();
    }

    public async Task<Result<FileRecord>> RequestFileById(Guid fileId)
    {
        var file = await GetFileById(fileId);
        if (file == null) return Result.Fail(new FileNotExistError());

        return Result.Ok(file);
    }
}