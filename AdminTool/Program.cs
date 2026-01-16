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

// Enhanced console output
Console.Clear();
Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine("╔══════════════════════════════════╗");
Console.WriteLine("║        Admin CLI Tool            ║");
Console.WriteLine("╚══════════════════════════════════╝");
Console.ResetColor();

Console.WriteLine();
Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine("Please select an option:");
Console.ResetColor();
Console.WriteLine();
Console.WriteLine("  [1] Generate authentication token");
Console.WriteLine("  [2] Create new user account");
Console.WriteLine();
Console.Write("Enter your choice (1-2): ");

string? option = Console.ReadLine();

if( option == "1" )
{
    await GenerateToken(authService);
}
else if( option == "2" )
{
    await GenerateUser(apiContext);
}

async static Task GenerateUser(ApiContext apiContext)
{
    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("═══ Create New User ═══");
    Console.ResetColor();
    Console.WriteLine();

    Console.Write("📧 Email: ");
    string? email = Console.ReadLine();

    Console.Write("👤 Name: ");
    string? name = Console.ReadLine();

    Console.Write("🔒 Password: ");
    string? password = Console.ReadLine();

    User user = new();
    user.Email = email;
    user.Name = name;
    user.Password = password;

    int usersCount = await apiContext.Users.Where(u => u.Email == email).CountAsync();

    if(usersCount > 0)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Email already exists!");
        Console.ResetColor();
    }
    else
    {
        await apiContext.Users.AddAsync(user);
        await apiContext.SaveChangesAsync();
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("✅ User created successfully!");
        Console.ResetColor();
    }
}

async static Task GenerateToken(AuthService authService)
{
    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.Blue;
    Console.WriteLine("═══ Generate Token ═══");
    Console.ResetColor();
    Console.WriteLine();

    Console.Write("📧 Email: ");
    string? email = Console.ReadLine();

    Console.Write("🔒 Password: ");
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
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("✅ Login successful!");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"🔑 Token: {result.Token}");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("❌ Login failed! Invalid credentials.");
                Console.ResetColor();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"❌ Error: {ex.Message}");
            Console.ResetColor();
        }
    }
    else
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("❌ Email and password are required!");
        Console.ResetColor();
    }
}