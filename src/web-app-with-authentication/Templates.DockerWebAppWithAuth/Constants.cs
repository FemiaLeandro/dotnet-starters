namespace Templates.DockerWebAppWithAuth
{
    public static class Constants
    {
        public const string AppSettingsLocalEnvironment = "Development-local";
        public const string AppSettingsDevEnvironment = "Development";
        // Add other environments that you create in appsettings.json
        public const string AppSettingsReleaseEnvironment = "Release";

        public static readonly string[] DevelopmentEnvironments = new[]
        {
            AppSettingsLocalEnvironment,
            AppSettingsDevEnvironment
        };
    }
}
