using Microsoft.AspNetCore.Mvc;
using Moq;
using FluentAssertions;
using ApiTemplateControllers.Controllers;
using ApiTemplateControllers.Models;
using ApiTemplateControllers.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ApiTemplateControllers.Tests.Controllers;

public class AuthControllerTests : IDisposable
{
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly ApiContext _context;
    private readonly AuthService _authService;
    private readonly AuthController _controller;
    private readonly DbContextOptions<ApiContext> _options;

    public AuthControllerTests()
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
        
        // Use real AuthService instead of mock for controller tests
        _authService = new AuthService(_context, _configurationMock.Object);
        _controller = new AuthController(_authService);
    }

    [Fact]
    public async Task Login_WithValidCredentials_ShouldReturnOkResult()
    {
        // Arrange
        var password = "testPassword123";
        var user = new User
        {
            Id = 1,
            Name = "Test User",
            Email = "test@example.com",
            HashedPassword = AuthService.HashPassword(password)
        };
        
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        
        var loginRequest = new LoginRequest
        {
            Email = "test@example.com",
            Password = password
        };
        
        // Act
        var result = await _controller.Login(loginRequest);
        
        // Assert
        result.Result.Should().BeOfType<OkObjectResult>();
        var okResult = result.Result as OkObjectResult;
        var loginResponse = okResult!.Value as LoginResponse;
        
        loginResponse.Should().NotBeNull();
        loginResponse!.Email.Should().Be("test@example.com");
        loginResponse.UserId.Should().Be(1);
        loginResponse.Token.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Login_WithInvalidCredentials_ShouldReturnUnauthorized()
    {
        // Arrange
        var password = "testPassword123";
        var user = new User
        {
            Id = 1,
            Name = "Test User",
            Email = "test@example.com",
            HashedPassword = AuthService.HashPassword(password)
        };
        
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        
        var loginRequest = new LoginRequest
        {
            Email = "test@example.com",
            Password = "wrongPassword"
        };
        
        // Act
        var result = await _controller.Login(loginRequest);
        
        // Assert
        result.Result.Should().BeOfType<UnauthorizedObjectResult>();
        var unauthorizedResult = result.Result as UnauthorizedObjectResult;
        unauthorizedResult!.Value.Should().Be("Invalid email or password");
    }

    [Fact]
    public async Task Login_WithEmptyEmail_ShouldReturnBadRequest()
    {
        // Arrange
        var loginRequest = new LoginRequest
        {
            Email = "",
            Password = "testPassword123"
        };
        
        // Act
        var result = await _controller.Login(loginRequest);
        
        // Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result.Result as BadRequestObjectResult;
        badRequestResult!.Value.Should().Be("Email and password are required");
    }

    [Fact]
    public async Task Login_WithEmptyPassword_ShouldReturnBadRequest()
    {
        // Arrange
        var loginRequest = new LoginRequest
        {
            Email = "test@example.com",
            Password = ""
        };
        
        // Act
        var result = await _controller.Login(loginRequest);
        
        // Assert
        result.Result.Should().BeOfType<BadRequestObjectResult>();
        var badRequestResult = result.Result as BadRequestObjectResult;
        badRequestResult!.Value.Should().Be("Email and password are required");
    }

    [Fact]
    public async Task Login_WithNonExistentUser_ShouldReturnUnauthorized()
    {
        // Arrange
        var loginRequest = new LoginRequest
        {
            Email = "nonexistent@example.com",
            Password = "anyPassword"
        };
        
        // Act
        var result = await _controller.Login(loginRequest);
        
        // Assert
        result.Result.Should().BeOfType<UnauthorizedObjectResult>();
        var unauthorizedResult = result.Result as UnauthorizedObjectResult;
        unauthorizedResult!.Value.Should().Be("Invalid email or password");
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}