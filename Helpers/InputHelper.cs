using System;
using System.IO;
using Synchronizing_two_folders.Models;

namespace Synchronizing_two_folders.Helpers
{
    public static class InputHelper
    {
        public static SyncConfig GetUserInput(string defaultSourcePath = null)
        {
            string sourcePath = string.Empty;
            string replicaPath, logFolderPath;
            int interval;

            // SOURCE PATH with default shown in brackets
            do
{
                if (!string.IsNullOrWhiteSpace(defaultSourcePath))
                {
                    Console.WriteLine($"Last used path: [{defaultSourcePath}]");
                }

                Console.Write("Source folder path: ");
                string input = Console.ReadLine();

                sourcePath = string.IsNullOrWhiteSpace(input) ? defaultSourcePath : input;

                if (!Directory.Exists(sourcePath))
                    Console.WriteLine("Error: Source path does not exist.");

            } while (!Directory.Exists(sourcePath)) ;

            // REPLICA PATH
            do
            {
                Console.Write("Replica folder path: ");
                replicaPath = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(replicaPath))
                    Console.WriteLine("Error: Replica path cannot be empty.");
            }
            while (string.IsNullOrWhiteSpace(replicaPath));

            // INTERVAL
            do
            {
                Console.Write("Synchronization interval (seconds): ");
                if (!int.TryParse(Console.ReadLine(), out interval) || interval <= 0)
                    Console.WriteLine("Error: Enter a positive number.");
            }
            while (interval <= 0);

            // LOG FOLDER
            do
            {
                Console.Write("Log folder path: ");
                logFolderPath = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(logFolderPath))
                    Console.WriteLine("Error: Log folder path cannot be empty.");
            }
            while (string.IsNullOrWhiteSpace(logFolderPath));

            string logFilePath = Path.Combine(logFolderPath, "synchronization.log");

            return new SyncConfig(new string[] { sourcePath, replicaPath, interval.ToString(), logFilePath });
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

            var json = System.Text.Json.JsonSerializer.Serialize(
                config,
                new System.Text.Json.JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(jsonPath, json);
        }
    }
}
