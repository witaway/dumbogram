using Dumbogram.Core.Users.Models;
using Dumbogram.Database;
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

    public async void CreateUserProfile(Guid userId, UserProfile userProfile)
    {
        userProfile.UserId = userId;
        await _dbContext.UserProfiles.AddAsync(userProfile);
        _dbContext.SaveChanges();
    }
}