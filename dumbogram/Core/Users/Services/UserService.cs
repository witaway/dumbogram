using Dumbogram.Core.Users.Errors;
using Dumbogram.Core.Users.Models;
using Dumbogram.Database;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Dumbogram.Core.Users.Services;

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

        if (userProfile == null)
        {
            const string message = "User not found";
            return Result.Fail(new UserNotFoundError(message));
        }

        return Result.Ok(userProfile);
    }

    public async Task CreateUserProfile(UserProfile userProfile)
    {
        await _dbContext.UserProfiles.AddAsync(userProfile);
        await _dbContext.SaveChangesAsync();
    }
}