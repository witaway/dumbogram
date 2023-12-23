using Dumbogram.Api.Application.Errors.Users;
using Dumbogram.Api.Persistence.Context.Application;
using Dumbogram.Api.Persistence.Context.Application.Entities.Users;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Dumbogram.Api.Domain.Services.Users;

public class UserService
{
    private readonly ApplicationDbContext _dbContext;

    public UserService(
        ApplicationDbContext dbContext
    )
    {
        _dbContext = dbContext;
    }

    public async Task<UserProfile?> ReadUserProfileById(Guid userId)
    {
        var user = await _dbContext.UserProfiles.SingleOrDefaultAsync(u => u.UserId == userId);
        return user;
    }

    public async Task<Result<UserProfile>> RequestUserProfileById(Guid userId)
    {
        var userProfile = await ReadUserProfileById(userId);

        if (userProfile == null) return Result.Fail(new UserNotFoundError());

        return Result.Ok(userProfile);
    }

    public async Task<bool> IsUserProfileWithUsernameExist(string username)
    {
        return await _dbContext
            .UserProfiles
            .AnyAsync(u => u.Username == username);
    }

    public async Task CreateUserProfile(UserProfile userProfile)
    {
        await _dbContext.UserProfiles.AddAsync(userProfile);
        await _dbContext.SaveChangesAsync();
    }
}