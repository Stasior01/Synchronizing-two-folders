using System;
using System.IO;
using Synchronizing_two_folders.Models;

namespace Synchronizing_two_folders.Helpers
{
    public static class InputHelper
    {
        public static SyncConfig GetUserInput()
        {
            string sourcePath, replicaPath, logFolderPath;
            int interval;

            do
            {
                Console.Write("Source folder path: ");
                sourcePath = Console.ReadLine();
                if (!Directory.Exists(sourcePath))
                    Console.WriteLine("Error: Source path does not exist. Please enter a valid path.");
            }
            while (!Directory.Exists(sourcePath));

            do
            {
                Console.Write("Replica folder path: ");
                replicaPath = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(replicaPath))
                    Console.WriteLine("Error: Replica path cannot be empty. Please enter a valid path.");
            }
            while (string.IsNullOrWhiteSpace(replicaPath));

            do
            {
                Console.Write("Synchronization interval (seconds): ");
                if (!int.TryParse(Console.ReadLine(), out interval) || interval <= 0)
                    Console.WriteLine("Error: Please enter a valid positive integer for interval.");
            }
            while (interval <= 0);

            do
            {
                Console.Write("Log folder path: ");
                logFolderPath = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(logFolderPath) || !Directory.Exists(logFolderPath))
                    Console.WriteLine("Error: Log folder path must exist and cannot be empty.");
            }
            while (string.IsNullOrWhiteSpace(logFolderPath) || !Directory.Exists(logFolderPath));

            string logFilePath = Path.Combine(logFolderPath, "synchronization.log");

            return new SyncConfig(new string[] { sourcePath, replicaPath, interval.ToString(), logFilePath });
        }
    }
}
