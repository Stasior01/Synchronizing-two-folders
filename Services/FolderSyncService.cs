using System;
using System.IO;
using Synchronizing_two_folders.Models;
using Serilog;

namespace Synchronizing_two_folders.Services
{
    public class FolderSyncService
    {
        private readonly SyncConfig _config;

        public FolderSyncService(SyncConfig config)
        {
            _config = config;
        }

        public void Synchronize()
        {
            try
            {
                if (!Directory.Exists(_config.SourcePath))
                {
                    Log.Warning("Source folder {SourcePath} does not exist.", _config.SourcePath);
                    return;
                }

                if (!Directory.Exists(_config.ReplicaPath))
                {
                    Directory.CreateDirectory(_config.ReplicaPath);
                    Log.Information("Created replica folder: {ReplicaPath}", _config.ReplicaPath);
                }

                CopyAll(new DirectoryInfo(_config.SourcePath), new DirectoryInfo(_config.ReplicaPath));
                RemoveExtra(new DirectoryInfo(_config.SourcePath), new DirectoryInfo(_config.ReplicaPath));

                Log.Information("Synchronization complete.");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error during synchronization");
            }
        }

        private void CopyAll(DirectoryInfo source, DirectoryInfo replica)
        {
            foreach (var dir in source.GetDirectories())
            {
                DirectoryInfo replicaSubDir = replica.CreateSubdirectory(dir.Name);
                CopyAll(dir, replicaSubDir);
            }

            foreach (var file in source.GetFiles())
            {
                string targetFilePath = Path.Combine(replica.FullName, file.Name);
                if (!File.Exists(targetFilePath) || file.LastWriteTime > File.GetLastWriteTime(targetFilePath))
                {
                    file.CopyTo(targetFilePath, true);
                    Log.Information("Copied/Updated file: {TargetFilePath}", targetFilePath);
                }
            }
        }

        private void RemoveExtra(DirectoryInfo source, DirectoryInfo replica)
        {
            foreach (var file in replica.GetFiles())
            {
                string sourceFilePath = Path.Combine(source.FullName, file.Name);
                if (!File.Exists(sourceFilePath))
                {
                    file.Delete();
                    Log.Information("Deleted extra file: {FilePath}", file.FullName);
                }
            }

            foreach (var dir in replica.GetDirectories())
            {
                string sourceDirPath = Path.Combine(source.FullName, dir.Name);
                if (!Directory.Exists(sourceDirPath))
                {
                    dir.Delete(true);
                    Log.Information("Deleted extra directory: {DirectoryPath}", dir.FullName);
                }
                else
                {
                    RemoveExtra(new DirectoryInfo(sourceDirPath), dir);
                }
            }
        }
    }
}
