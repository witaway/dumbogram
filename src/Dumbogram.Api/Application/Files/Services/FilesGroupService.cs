using Dumbogram.Api.Application.Chats.Services.Errors;
using Dumbogram.Api.Application.Files.Services.Errors;
using Dumbogram.Api.Persistence.Context.Application;
using Dumbogram.Api.Persistence.Context.Application.Entities.Files;
using Dumbogram.Api.Persistence.Context.Application.Entities.Users;
using Dumbogram.Api.Persistence.Context.Application.Enumerations;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Dumbogram.Api.Application.Files.Services;

public class FilesGroupService
{
    private readonly ApplicationDbContext _dbContext;

    private readonly FileRecordService _fileRecordService;

    public FilesGroupService(
        FileRecordService fileRecordService,
        ApplicationDbContext dbContext
    )
    {
        _fileRecordService = fileRecordService;
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

        if (filesGroup == null) return Result.Fail(new FilesGroupNotExistError());

        return Result.Ok(filesGroup);
    }

    public async Task<Result<FilesGroup>> RequestOwnedFilesGroupById(UserProfile subjectUser, Guid filesGroupId)
    {
        var filesGroup = await GetFilesGroupById(filesGroupId);

        if (filesGroup == null) return Result.Fail(new FilesGroupNotExistError());

        if (filesGroup.Owner != subjectUser) return Result.Fail(new NotEnoughRightsError());

        return Result.Ok(filesGroup);
    }

    public async Task AddFileToFilesGroup(FilesGroup filesGroup, FileRecord fileRecord)
    {
        filesGroup.Files.Add(fileRecord);
        await _dbContext.SaveChangesAsync();
    }

    public async Task AddFilesRangeToFilesGroup(FilesGroup filesGroup, IEnumerable<FileRecord> files)
    {
        foreach (var file in files) filesGroup.Files.Add(file);

        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveFileFromFilesGroup(FilesGroup filesGroup, FileRecord fileRecord)
    {
        filesGroup.Files.Remove(fileRecord);
        await _dbContext.SaveChangesAsync();
    }
}