namespace SnipeItTests.Config;

// Everything that might change between environments lives here.
public static class Settings
{
    public static string BaseUrl => Env("SNIPEIT_BASE_URL", "https://demo.snipeitapp.com").TrimEnd('/');
    public static string Username => Env("SNIPEIT_USERNAME", "admin");
    public static string Password => Env("SNIPEIT_PASSWORD", "password");

    private static string Env(string name, string fallback)
    {
        var value = Environment.GetEnvironmentVariable(name);
        return string.IsNullOrWhiteSpace(value) ? fallback : value;
    }
}
