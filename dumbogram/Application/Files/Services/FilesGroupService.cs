using Dumbogram.Application.Files.Services.Errors;
using Dumbogram.Database;
using Dumbogram.Models.Files;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using File = Dumbogram.Models.Files.File;

namespace Dumbogram.Application.Files.Services;

public class FilesGroupService
{
    private readonly ApplicationDbContext _dbContext;

    private readonly FileService _fileService;

    public FilesGroupService(
        FileService fileService,
        ApplicationDbContext dbContext
    )
    {
        _fileService = fileService;
        _dbContext = dbContext;
    }

    public async Task CreateFilesGroup(FilesGroup filesGroup)
    {
        _dbContext.FilesGroups.Add(filesGroup);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteFilesGroup(FilesGroup filesGroup)
    {
        _dbContext.FilesGroups.Remove(filesGroup);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<FilesGroup?> GetFilesGroupById(Guid filesGroupId)
    {
        var query = _dbContext
            .FilesGroups
            .Include(filesGroup => filesGroup.Files)
            .Where(filesGroup => filesGroup.Id == filesGroupId);

        return await query.SingleOrDefaultAsync();
    }

    public async Task<Result<FilesGroup>> RequestFilesGroupById(Guid filesGroupId)
    {
        var filesGroup = await GetFilesGroupById(filesGroupId);
        if (filesGroup == null)
        {
            return Result.Fail(new FilesGroupNotExistError());
        }

        return Result.Ok(filesGroup);
    }

    public async Task AddFileToFilesGroup(FilesGroup filesGroup, File file)
    {
        filesGroup.Files.Add(file);
        await _dbContext.SaveChangesAsync();
    }
}