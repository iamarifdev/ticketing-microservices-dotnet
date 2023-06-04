using Ticketing.Auth.Application.Services;
using Ticketing.Auth.Domain.Entities;
using Ticketing.Auth.Persistence;

namespace Ticketing.Auth.Tests;

public static class TestDataHelper
{
    public const string TestPassword = "test";
    public const string TestEmail = "test@test.com";
    
    public static async Task SeedTestData(this AuthDbContext dbContext)
    {
        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();

        var passwordHash = await PasswordService.ToHash(TestPassword);
        var user = new User
        {
            Email = TestEmail,
            FirstName = "Test",
            LastName = "User",
            Password = passwordHash
        };

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();
    }
}