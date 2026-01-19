# 🚀 API Template Controllers

![Tests](https://github.com/Oliver98t/ApiTemplateControllers/actions/workflows/simple-tests.yml/badge.svg)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

A comprehensive .NET 8 Web API template with JWT authentication, Entity Framework, and CRUD operations. Perfect for building modern RESTful APIs with built-in security and testing infrastructure.

## 📋 Table of Contents

- [Features](#-features)
- [Project Structure](#-project-structure)
- [Prerequisites](#-prerequisites)
- [Quick Start](#-quick-start)
- [Configuration](#-configuration)
- [Authentication](#-authentication)
- [API Endpoints](#-api-endpoints)
- [Database Operations](#-database-operations)
- [Testing](#-testing)
- [Development](#-development)
- [Deployment](#-deployment)
- [Contributing](#-contributing)

## ✨ Features

- 🔐 **JWT Authentication** - Secure token-based authentication
- 🗄️ **Entity Framework Core** - Code-first database approach
- 🏗️ **Generic CRUD Operations** - Reusable data access patterns
- 📝 **Comprehensive Logging** - Built-in logging and monitoring
- 🧪 **Unit Testing** - Full test coverage with xUnit
- 🔄 **CI/CD Pipeline** - Automated testing with GitHub Actions
- 📚 **OpenAPI/Swagger** - Auto-generated API documentation
- 🔒 **Password Hashing** - BCrypt secure password storage
- 🎯 **CORS Support** - Cross-origin resource sharing
- 📦 **Docker Ready** - Container deployment support

## 📁 Project Structure

```
ApiTemplateControllers/
├── 📂 ApiTemplateControllers/          # Main API project
│   ├── 📂 Controllers/                 # API controllers
│   │   ├── AuthController.cs          # Authentication endpoints
│   │   ├── BaseController.cs          # Generic CRUD controller
│   │   └── Controllers.cs             # Specific entity controllers
│   ├── 📂 Models/                     # Data models and DTOs
│   │   ├── ApiContext.cs              # Entity Framework context
│   │   ├── AuthModels.cs              # Authentication models
│   │   ├── BaseModel.cs               # Base entity interface
│   │   └── Models.cs                  # Application entities
│   ├── 📂 Services/                   # Business logic layer
│   │   ├── AuthService.cs             # Authentication services
│   │   ├── BaseServices.cs            # Generic business services
│   │   ├── CRUD.cs                    # Generic CRUD operations
│   │   └── Services.cs                # Specific business logic
│   ├── 📂 Migrations/                 # Entity Framework migrations
│   └── Program.cs                     # Application entry point
├── 📂 AdminTool/                      # Database administration tool
│   └── Program.cs                     # CLI admin utilities
├── 📂 ApiTemplateControllers.Tests/   # Unit tests project
└── 📂 .github/workflows/              # CI/CD automation
```

## 🔧 Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- [Entity Framework Core Tools](https://docs.microsoft.com/en-us/ef/core/cli/dotnet)
- Database server (SQLite included for development)
- [Visual Studio](https://visualstudio.microsoft.com/) / [VS Code](https://code.visualstudio.com/) (recommended)

## 🚀 Quick Start

### 1. Clone the Repository

```bash
git clone https://github.com/Oliver98t/ApiTemplateControllers.git
cd ApiTemplateControllers
```

### 2. Install Dependencies

```bash
# Restore NuGet packages
dotnet restore

# Install EF Core tools (if not already installed)
dotnet tool install --global dotnet-ef
```

### 3. Configure Database

```bash
# Navigate to the main project
cd ApiTemplateControllers

# Create initial database migration
dotnet ef migrations add Initial

# Apply migrations to create database
dotnet ef database update
```

### 4. Run the Application

```bash
# Start the API server
dotnet run

# The API will be available at:
# - HTTP: http://localhost:5000
# - HTTPS: https://localhost:5001
# - Swagger UI: https://localhost:5001/swagger
```

### 5. Run Tests

```bash
# Run all unit tests
dotnet test

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"
```

## ⚙️ Configuration

### Application Settings

Configure the application via `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=app.db"
  },
  "Jwt": {
    "SecretKey": "your-super-secret-key-here-make-it-long-and-random",
    "Issuer": "ApiTemplateControllers",
    "Audience": "ApiTemplateUsers"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

### Environment Variables

For production, set these environment variables:

```bash
export ConnectionStrings__DefaultConnection="your-production-db-connection"
export Jwt__SecretKey="your-production-secret-key"
export ASPNETCORE_ENVIRONMENT="Production"
```

## 🔐 Authentication

The API uses JWT (JSON Web Tokens) for authentication.

### Register a New User

```bash
curl -X POST https://localhost:5001/api/users \\
  -H "Content-Type: application/json" \\
  -d '{
    "name": "John Doe",
    "email": "john@example.com",
    "password": "SecurePassword123"
  }'
```

### Login

```bash
curl -X POST https://localhost:5001/api/auth/login \\
  -H "Content-Type: application/json" \\
  -d '{
    "email": "john@example.com",
    "password": "SecurePassword123"
  }'
```

### Using the Token

Include the JWT token in subsequent requests:

```bash
curl -X GET https://localhost:5001/api/users \\
  -H "Authorization: Bearer YOUR_JWT_TOKEN_HERE"
```

## 📡 API Endpoints

### Authentication Endpoints

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| `POST` | `/api/auth/login` | User login | ❌ |

### User Management

| Method | Endpoint | Description | Auth Required |
|--------|----------|-------------|---------------|
| `GET` | `/api/users` | Get all users | ✅ |
| `GET` | `/api/users/{id}` | Get user by ID | ✅ |
| `POST` | `/api/users` | Create new user | ❌ |
| `PUT` | `/api/users/{id}` | Update user | ✅ |
| `DELETE` | `/api/users/{id}` | Delete user | ✅ |

### Generic Entity Operations

The API provides generic CRUD operations for any entity that implements `IBaseModel`.

## 🗄️ Database Operations

### Entity Framework Migrations

```bash
# Create a new migration
dotnet ef migrations add AddNewFeature

# Apply migrations to database
dotnet ef database update

# Remove last migration (if not applied)
dotnet ef migrations remove

# View migration history
dotnet ef migrations list
```

### Admin Tool

Use the included admin tool for database management:

```bash
cd AdminTool
dotnet run
```

## 🧪 Testing

### Running Tests

```bash
# Run all tests
dotnet test

# Run tests with detailed output
dotnet test --verbosity normal

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage" --results-directory ./coverage
```

### Test Structure

- **Unit Tests**: Test individual components in isolation
- **Integration Tests**: Test API endpoints and database operations
- **Authentication Tests**: Test JWT token generation and validation

### Writing Tests

```csharp
[Fact]
public async Task LoginAsync_ValidCredentials_ReturnsToken()
{
    // Arrange
    var authService = new AuthService(_context, _configuration);
    var request = new LoginRequest { Email = "test@example.com", Password = "password" };
    
    // Act
    var result = await authService.LoginAsync(request);
    
    // Assert
    Assert.NotNull(result);
    Assert.NotEmpty(result.Token);
}
```

## 💻 Development

### Code Style

- Follow [C# Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/inside-a-program/coding-conventions)
- Use XML documentation for public APIs
- Implement proper error handling
- Write unit tests for new features

### Adding New Entities

1. **Create Model**: Add new entity in `Models/Models.cs`
2. **Update DbContext**: Add `DbSet<YourEntity>` to `ApiContext.cs`
3. **Create Migration**: Run `dotnet ef migrations add AddYourEntity`
4. **Apply Migration**: Run `dotnet ef database update`
5. **Add Controller**: Inherit from `Controller<YourEntity>` for CRUD operations

### Adding New Services

1. **Create Service**: Add new service class in `Services/`
2. **Register Service**: Add to dependency injection in `Program.cs`
3. **Write Tests**: Create corresponding test class
4. **Document**: Add XML documentation

## 🚢 Deployment

### Docker Deployment

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ApiTemplateControllers/ApiTemplateControllers.csproj", "ApiTemplateControllers/"]
RUN dotnet restore "ApiTemplateControllers/ApiTemplateControllers.csproj"
COPY . .
WORKDIR "/src/ApiTemplateControllers"
RUN dotnet build "ApiTemplateControllers.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ApiTemplateControllers.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ApiTemplateControllers.dll"]
```

### Build and Run

```bash
# Build Docker image
docker build -t api-template-controllers .

# Run container
docker run -p 8080:80 api-template-controllers
```

### Production Deployment

1. **Configure Production Settings**: Update `appsettings.Production.json`
2. **Set Environment Variables**: Configure connection strings and secrets
3. **Build for Production**: `dotnet publish -c Release`
4. **Deploy**: Use your preferred hosting platform (Azure, AWS, etc.)

## 🤝 Contributing

1. **Fork the repository**
2. **Create feature branch**: `git checkout -b feature/amazing-feature`
3. **Make changes** and add tests
4. **Run tests**: `dotnet test`
5. **Commit changes**: `git commit -m 'Add amazing feature'`
6. **Push to branch**: `git push origin feature/amazing-feature`
7. **Create Pull Request**

### Development Workflow

- All changes must have tests
- All tests must pass
- Follow coding conventions
- Update documentation as needed
- Use meaningful commit messages

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- **Entity Framework Core** - Data access technology
- **JWT** - Authentication standard
- **xUnit** - Testing framework
- **Swagger/OpenAPI** - API documentation
- **BCrypt.NET** - Password hashing library

## 📞 Support

- **Issues**: [GitHub Issues](https://github.com/Oliver98t/ApiTemplateControllers/issues)
- **Documentation**: Check the `/swagger` endpoint when running locally
- **Author**: Oliver Tattersfield

---

## 📚 Additional Resources

- [.NET Web API Documentation](https://docs.microsoft.com/en-us/aspnet/core/web-api/)
- [Entity Framework Core Documentation](https://docs.microsoft.com/en-us/ef/core/)
- [JWT Introduction](https://jwt.io/introduction/)
- [xUnit Testing Documentation](https://xunit.net/)

---

**Happy coding! 🎉**