using System.ComponentModel.DataAnnotations;
using ApiTemplateControllers.Models;
using FluentAssertions;

namespace ApiTemplateControllers.Tests.Models;

public class ModelTests
{
    [Fact]
    public void User_WithValidEmail_ShouldPassValidation()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Name = "Test User",
            Email = "test@example.com",
            HashedPassword = "hashedPassword"
        };

        // Act
        var validationResults = ValidateModel(user);

        // Assert
        validationResults.Should().BeEmpty();
    }

    [Fact]
    public void User_WithNullEmail_ShouldFailValidation()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Name = "Test User",
            Email = null,
            HashedPassword = "hashedPassword"
        };

        // Act
        var validationResults = ValidateModel(user);

        // Assert
        validationResults.Should().NotBeEmpty();
        validationResults.Should().Contain(r => r.ErrorMessage != null && r.ErrorMessage.Contains("required"));
    }

    [Fact]
    public void LoginRequest_Properties_ShouldHaveCorrectDefaultValues()
    {
        // Arrange & Act
        var loginRequest = new LoginRequest();

        // Assert
        loginRequest.Email.Should().Be(string.Empty);
        loginRequest.Password.Should().Be(string.Empty);
    }

    [Fact]
    public void LoginResponse_Properties_ShouldHaveCorrectDefaultValues()
    {
        // Arrange & Act
        var loginResponse = new LoginResponse();

        // Assert
        loginResponse.Token.Should().Be(string.Empty);
        loginResponse.Email.Should().Be(string.Empty);
        loginResponse.UserId.Should().Be(0);
        loginResponse.ExpiresAt.Should().Be(default);
    }

    [Fact]
    public void UserInput_ShouldImplementIBaseModel()
    {
        // Arrange & Act
        var userInput = new UserInput();

        // Assert
        userInput.Should().BeAssignableTo<IBaseModel>();
        userInput.Id.Should().Be(0);
    }

    [Fact]
    public void Item_ShouldImplementIBaseModel()
    {
        // Arrange & Act
        var item = new Item();

        // Assert
        item.Should().BeAssignableTo<IBaseModel>();
        item.Id.Should().Be(0);
        item.String.Should().BeNull();
        item.Int.Should().Be(0);
    }

    private static List<ValidationResult> ValidateModel(object model)
    {
        var validationResults = new List<ValidationResult>();
        var validationContext = new ValidationContext(model, null, null);
        Validator.TryValidateObject(model, validationContext, validationResults, true);
        return validationResults;
    }
}