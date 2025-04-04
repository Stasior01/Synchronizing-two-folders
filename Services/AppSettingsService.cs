using Microsoft.Extensions.Configuration;
using System.IO;
using System.Text.Json;

namespace Synchronizing_two_folders.Services
{
    public static class AppSettingsService
    {
        public static string LoadLastUsedSource()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .Build();

            return config["LastUsed:SourcePath"];
        }

        public static void SaveLastUsedSource(string sourcePath)
        {
            var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");

            var config = new
            {
                LastUsed = new
                {
                    SourcePath = sourcePath
                }
            };

            var json = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(jsonPath, json);
        }
    }
}
