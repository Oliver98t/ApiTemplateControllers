using ApiTemplateControllers.Models;
using ApiTemplateControllers.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

var configPath = Path.GetFullPath("../ApiTemplateControllers/appsettings.json");
Console.WriteLine($"DIR: {configPath}");

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile(configPath, optional: false)
    .Build();

var services = new ServiceCollection();
services.AddSingleton<IConfiguration>(configuration);
services.AddDbContext<ApiContext>(opt =>
    opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
services.AddScoped<AuthService>();

var serviceProvider = services.BuildServiceProvider();
var authService = serviceProvider.GetRequiredService<AuthService>();
var apiContext = serviceProvider.GetRequiredService<ApiContext>();

Console.WriteLine("------Admin CLI tool------");
Console.WriteLine("\n\nOptions:\n1: Generate token\n2: Create new user");

string? option = Console.ReadLine();

if( option == "1" )
{
    GenerateToken(authService);
}
else if( option == "2" )
{
    GenerateUser(apiContext);
}

async static void GenerateUser(ApiContext apiContext)
{
    Console.WriteLine("Input Email: ");
    string? email = Console.ReadLine();

    Console.WriteLine("Input name: ");
    string? name = Console.ReadLine();

    Console.WriteLine("Input password: ");
    string? password = Console.ReadLine();

    User user = new();
    user.Email = email;
    user.Name = name;
    user.Password = password;

    apiContext.Users.Add(user);
    await apiContext.SaveChangesAsync();
}

async static void GenerateToken(AuthService authService)
{
    Console.WriteLine("Input Email: ");
    string? email = Console.ReadLine();

    Console.WriteLine("Input password: ");
    string? password = Console.ReadLine();

    if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
    {
        var loginRequest = new LoginRequest
        {
            Email = email,
            Password = password
        };

        try
        {
            var result = await authService.LoginAsync(loginRequest);
            if (result != null)
            {
                Console.WriteLine($"Token: {result.Token}");
            }
            else
            {
                Console.WriteLine("Login failed!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}