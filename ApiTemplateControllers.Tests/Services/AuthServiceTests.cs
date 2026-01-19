using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using FluentAssertions;
using ApiTemplateControllers.Models;
using ApiTemplateControllers.Services;

namespace ApiTemplateControllers.Tests.Services;

public class AuthServiceTests : IDisposable
{
    private readonly ApiContext _context;
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly AuthService _authService;
    private readonly DbContextOptions<ApiContext> _options;

    public AuthServiceTests()
    {
        // Setup in-memory database
        _options = new DbContextOptionsBuilder<ApiContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        
        _context = new ApiContext(_options);
        
        // Setup configuration mock
        _configurationMock = new Mock<IConfiguration>();
        _configurationMock.Setup(x => x["Jwt:SecretKey"]).Returns("SuperSecretKeyThatIsAtLeast32CharactersLong123456789");
        _configurationMock.Setup(x => x["Jwt:Issuer"]).Returns("TestIssuer");
        _configurationMock.Setup(x => x["Jwt:Audience"]).Returns("TestAudience");
        
        _authService = new AuthService(_context, _configurationMock.Object);
    }

    [Fact]
    public void HashPassword_ShouldReturnHashedPassword()
    {
        // Arrange
        var password = "testPassword123";
        
        // Act
        var hashedPassword = AuthService.HashPassword(password);
        
        // Assert
        hashedPassword.Should().NotBeNullOrEmpty();
        hashedPassword.Should().NotBe(password);
        hashedPassword.Should().StartWith("$2a$12$"); // BCrypt hash format with work factor 12
    }

    [Fact]
    public void VerifyPassword_WithCorrectPassword_ShouldReturnTrue()
    {
        // Arrange
        var password = "testPassword123";
        var hashedPassword = AuthService.HashPassword(password);
        
        // Act
        var result = AuthService.VerifyPassword(password, hashedPassword);
        
        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void VerifyPassword_WithIncorrectPassword_ShouldReturnFalse()
    {
        // Arrange
        var password = "testPassword123";
        var wrongPassword = "wrongPassword456";
        var hashedPassword = AuthService.HashPassword(password);
        
        // Act
        var result = AuthService.VerifyPassword(wrongPassword, hashedPassword);
        
        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task LoginAsync_WithValidCredentials_ShouldReturnLoginResponse()
    {
        // Arrange
        var password = "testPassword123";
        var hashedPassword = AuthService.HashPassword(password);
        var user = new User
        {
            Id = 1,
            Name = "Test User",
            Email = "test@example.com",
            HashedPassword = hashedPassword
        };
        
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        
        var loginRequest = new LoginRequest
        {
            Email = "test@example.com",
            Password = password
        };
        
        // Act
        var result = await _authService.LoginAsync(loginRequest);
        
        // Assert
        result.Should().NotBeNull();
        result!.Email.Should().Be("test@example.com");
        result.UserId.Should().Be(1);
        result.Token.Should().NotBeNullOrEmpty();
        result.ExpiresAt.Should().BeCloseTo(DateTime.UtcNow.AddHours(24), TimeSpan.FromMinutes(1));
    }

    [Fact]
    public async Task LoginAsync_WithInvalidEmail_ShouldReturnNull()
    {
        // Arrange
        var loginRequest = new LoginRequest
        {
            Email = "nonexistent@example.com",
            Password = "anyPassword"
        };
        
        // Act
        var result = await _authService.LoginAsync(loginRequest);
        
        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task LoginAsync_WithInvalidPassword_ShouldReturnNull()
    {
        // Arrange
        var password = "testPassword123";
        var hashedPassword = AuthService.HashPassword(password);
        var user = new User
        {
            Id = 1,
            Name = "Test User",
            Email = "test@example.com",
            HashedPassword = hashedPassword
        };
        
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        
        var loginRequest = new LoginRequest
        {
            Email = "test@example.com",
            Password = "wrongPassword"
        };
        
        // Act
        var result = await _authService.LoginAsync(loginRequest);
        
        // Assert
        result.Should().BeNull();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}