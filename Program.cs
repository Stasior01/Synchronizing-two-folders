using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Serilog;
using Synchronizing_two_folders.Helpers;
using Synchronizing_two_folders.Models;
using Synchronizing_two_folders.Services;

class Program
{
    static volatile int syncIntervalSeconds = 10;
    static volatile bool exitRequested = false;
    static volatile bool forceSync = false;

    static void Main(string[] args)
    {
        // Load last used source path
        string lastUsedSourcePath = AppSettingsService.LoadLastUsedSource();

        // Get configuration from user or args
        SyncConfig config = args.Length != 4
            ? InputHelper.GetUserInput(lastUsedSourcePath)
            : new SyncConfig(args);

        if (!config.IsValid())
        {
            Console.WriteLine("Invalid configuration. Exiting.");
            return;
        }

        AppSettingsService.SaveLastUsedSource(config.SourcePath);
        FileSystemHelper.EnsureDirectoryExists(config.LogFilePath);
        LoggerService.ConfigureLogger(config.LogFilePath);

        Log.Information("Application started.");

        syncIntervalSeconds = config.SyncIntervalSeconds;
        var syncService = new FolderSyncService(config);

        // Start background synchronization loop
        Task.Run(() => RunBackgroundSync(syncService));

        // Start interactive menu
        var menu = new MenuService(
            forceSync: () => forceSync = true,
            changeInterval: newVal => syncIntervalSeconds = newVal,
            exit: () => exitRequested = true
        );

        while (!exitRequested)
        {
            menu.ShowMenu();
        }

        Log.Information("Application exiting...");
        Log.CloseAndFlush();
    }

    static void RunBackgroundSync(FolderSyncService syncService)
    {
        while (!exitRequested)
        {
            syncService.Synchronize();

            for (int i = 0; i < syncIntervalSeconds * 10 && !exitRequested && !forceSync; i++)
            {
                Thread.Sleep(100);
            }

            if (forceSync)
                forceSync = false;
        }
    }
}
