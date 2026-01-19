# ApiTemplateControllers.Tests

Unit and integration tests for the ApiTemplateControllers project.

## Project Structure

```
ApiTemplateControllers.Tests/
├── Controllers/           # Controller unit tests
├── Services/             # Service unit tests
├── Integration/          # Integration tests
├── Models/              # Model validation tests
├── Helpers/             # Test data helpers
├── Configuration/       # Test configuration utilities
└── GlobalUsings.cs      # Global using statements
```

## Test Categories

### Unit Tests
- **AuthServiceTests**: Tests for authentication service logic
- **AuthControllerTests**: Tests for authentication controller endpoints
- **ModelTests**: Tests for model validation and behavior

### Integration Tests
- **AuthIntegrationTests**: End-to-end API testing with real HTTP requests

## Running Tests

### Using .NET CLI

```bash
# Run all tests
dotnet test

# Run tests with detailed output
dotnet test --verbosity normal

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Run specific test class
dotnet test --filter "AuthServiceTests"

# Run specific test method
dotnet test --filter "Login_WithValidCredentials_ShouldReturnOkResult"
```

### Using the Test Script

```bash
# Make script executable (Linux/macOS)
chmod +x ../run-tests.sh

# Run the script
../run-tests.sh
```

## Test Frameworks and Libraries

- **xUnit**: Primary test framework
- **FluentAssertions**: Expressive assertions
- **Moq**: Mocking framework
- **Microsoft.AspNetCore.Mvc.Testing**: Integration testing
- **Microsoft.EntityFrameworkCore.InMemory**: In-memory database for testing
- **coverlet.collector**: Code coverage collection

## Writing New Tests

### Unit Test Example

```csharp
[Fact]
public void Method_WithValidInput_ShouldReturnExpectedResult()
{
    // Arrange
    var input = "test input";
    var expected = "expected output";
    
    // Act
    var result = ServiceMethod(input);
    
    // Assert
    result.Should().Be(expected);
}
```

### Integration Test Example

```csharp
[Fact]
public async Task API_WithValidRequest_ShouldReturnSuccessResponse()
{
    // Arrange
    var request = new { Property = "value" };
    var json = JsonSerializer.Serialize(request);
    var content = new StringContent(json, Encoding.UTF8, "application/json");
    
    // Act
    var response = await _client.PostAsync("/api/endpoint", content);
    
    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.OK);
}
```

## Test Database

Tests use an in-memory Entity Framework database that is:
- Created fresh for each test class
- Seeded with test data as needed
- Automatically disposed after tests complete

## Mocking Strategy

- **Controllers**: Use real services with in-memory database
- **Services**: Mock external dependencies (configuration, etc.)
- **Integration**: Use TestServer with in-memory database

## Coverage Goals

- **Services**: 90%+ code coverage
- **Controllers**: 85%+ code coverage
- **Models**: 100% validation logic coverage
- **Integration**: All happy paths and critical error scenarios
