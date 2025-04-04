using Synchronizing_two_folders.Helpers;
using Synchronizing_two_folders.Models;
using Synchronizing_two_folders.Services;

class Program
{
    static void Main(string[] args)
    {
        SyncConfig config;

        if (args.Length != 4)
        {
            Console.WriteLine("Arguments not provided or incomplete. Please enter them manually:");
            config = InputHelper.GetUserInput();
        }
        else
        {
            config = new SyncConfig(args);
        }

        if (!config.IsValid())
        {
            Console.WriteLine("Invalid configuration provided. Application will terminate.");
            return;
        }

        var logger = new Logger(config.LogFilePath);
        var syncService = new FolderSyncService(config, logger);

        bool continueSync = true;
        while (continueSync)
        {
            syncService.Synchronize();
            Thread.Sleep(config.SyncIntervalSeconds * 1000);

            Console.WriteLine("\nPress ESC to exit or 1 to synchronize again.");
            var key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.Escape)
            {
                continueSync = false;
            }
            else if (key.Key == ConsoleKey.D1 || key.Key == ConsoleKey.NumPad1)
            {
                continueSync = true;
            }
            else
            {
                Console.WriteLine("Invalid input. Exiting program.");
                continueSync = false;
            }
        }
    }
}