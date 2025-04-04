using System;
using System.IO;

namespace Synchronizing_two_folders.Helpers
{
    public static class FileSystemHelper
    {
        public static void EnsureDirectoryExists(string path)
        {
            try
            {
                string? dir = Path.GetDirectoryName(path);
                if (!string.IsNullOrWhiteSpace(dir) && !Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to create directory: {ex.Message}");
                Environment.Exit(1);
            }
        }
    }
}
