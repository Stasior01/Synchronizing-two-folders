using Synchronizing_two_folders.Helpers;
using Synchronizing_two_folders.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synchronizing_two_folders.Services
{
    public class FolderSyncService
    {
        private readonly SyncConfig _config;
        private readonly Logger _logger;

        public FolderSyncService(SyncConfig config, Logger logger)
        {
            _config = config;
            _logger = logger;
        }

        public void Synchronize()
        {
            try
            {
                if (!Directory.Exists(_config.SourcePath))
                {
                    _logger.Log($"Source folder {_config.SourcePath} does not exist.");
                    return;
                }

                if (!Directory.Exists(_config.ReplicaPath))
                {
                    Directory.CreateDirectory(_config.ReplicaPath);
                    _logger.Log($"Created replica folder: {_config.ReplicaPath}");
                }

                CopyAll(new DirectoryInfo(_config.SourcePath), new DirectoryInfo(_config.ReplicaPath));
                RemoveExtra(new DirectoryInfo(_config.SourcePath), new DirectoryInfo(_config.ReplicaPath));

                _logger.Log("Synchronization complete.");
            }
            catch (Exception ex)
            {
                _logger.Log($"Error during synchronization: {ex.Message}");
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
                    _logger.Log($"Copied/Updated file: {targetFilePath}");
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
                    _logger.Log($"Deleted extra file: {file.FullName}");
                }
            }

            foreach (var dir in replica.GetDirectories())
            {
                string sourceDirPath = Path.Combine(source.FullName, dir.Name);
                if (!Directory.Exists(sourceDirPath))
                {
                    dir.Delete(true);
                    _logger.Log($"Deleted extra directory: {dir.FullName}");
                }
                else
                {
                    RemoveExtra(new DirectoryInfo(sourceDirPath), dir);
                }
            }
        }
    }

}
