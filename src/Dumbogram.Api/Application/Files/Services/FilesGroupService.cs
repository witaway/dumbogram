using Dumbogram.Api.Application.Chats.Services.Errors;
using Dumbogram.Api.Application.Files.Services.Errors;
using Dumbogram.Api.Database;
using Dumbogram.Api.Models.Files;
using Dumbogram.Api.Models.Users;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using File = Dumbogram.Api.Models.Files.File;

namespace Dumbogram.Api.Application.Files.Services;

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

    public async Task<FilesGroup> CreateFilesGroup(UserProfile subjectUser, FilesGroupType groupType)
    {
        var filesGroup = new FilesGroup
        {
            Owner = subjectUser,
            GroupType = groupType
        };
        _dbContext.FilesGroups.Add(filesGroup);
        await _dbContext.SaveChangesAsync();

        return (await GetFilesGroupById(filesGroup.Id))!;
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

    public async Task<Result<FilesGroup>> RequestOwnedFilesGroupById(UserProfile subjectUser, Guid filesGroupId)
    {
        var filesGroup = await GetFilesGroupById(filesGroupId);

        if (filesGroup == null)
        {
            return Result.Fail(new FilesGroupNotExistError());
        }

        if (filesGroup.Owner != subjectUser)
        {
            return Result.Fail(new NotEnoughRightsError());
        }

        return Result.Ok(filesGroup);
    }

    public async Task AddFileToFilesGroup(FilesGroup filesGroup, File file)
    {
        filesGroup.Files.Add(file);
        await _dbContext.SaveChangesAsync();
    }

    public async Task AddFilesRangeToFilesGroup(FilesGroup filesGroup, IEnumerable<File> files)
    {
        foreach (var file in files)
        {
            filesGroup.Files.Add(file);
        }

        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveFileFromFilesGroup(FilesGroup filesGroup, File file)
    {
        filesGroup.Files.Remove(file);
        await _dbContext.SaveChangesAsync();
    }
}