using ApiTemplateControllers.Models;
using ApiTemplateControllers.Services;

namespace ApiTemplateControllers.Tests.Helpers;

public static class TestDataHelper
{
    public static User CreateTestUser(long id = 1, string? name = null, string? email = null, string? password = null)
    {
        return new User
        {
            Id = id,
            Name = name ?? "Test User",
            Email = email ?? "test@example.com",
            HashedPassword = password != null ? AuthService.HashPassword(password) : AuthService.HashPassword("testPassword123")
        };
    }

    public static LoginRequest CreateLoginRequest(string? email = null, string? password = null)
    {
        return new LoginRequest
        {
            Email = email ?? "test@example.com",
            Password = password ?? "testPassword123"
        };
    }

    public static UserInput CreateUserInput(long id = 1, string? name = null, string? email = null, string? password = null)
    {
        return new UserInput
        {
            Id = id,
            Name = name ?? "Test User",
            Email = email ?? "test@example.com",
            Password = password ?? "testPassword123"
        };
    }

    public static Item CreateTestItem(long id = 1, string? str = null, int intValue = 42)
    {
        return new Item
        {
            Id = id,
            String = str ?? "Test Item",
            Int = intValue
        };
    }
}