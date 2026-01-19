using Microsoft.Extensions.Configuration;

namespace ApiTemplateControllers.Tests.Configuration;

public static class TestConfiguration
{
    public static IConfiguration GetConfiguration()
    {
        var inMemorySettings = new Dictionary<string, string?>
        {
            {"Jwt:SecretKey", "SuperSecretKeyThatIsAtLeast32CharactersLong123456789"},
            {"Jwt:Issuer", "TestIssuer"},
            {"Jwt:Audience", "TestAudience"},
            {"ConnectionStrings:DefaultConnection", "Data Source=:memory:"},
            {"Logging:LogLevel:Default", "Information"},
            {"AllowedHosts", "*"}
        };

        return new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();
    }

    public static class JwtSettings
    {
        public const string SecretKey = "SuperSecretKeyThatIsAtLeast32CharactersLong123456789";
        public const string Issuer = "TestIssuer";
        public const string Audience = "TestAudience";
    }
}