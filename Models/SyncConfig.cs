using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synchronizing_two_folders.Models
{
    public class SyncConfig
    {
        public string SourcePath { get; }
        public string ReplicaPath { get; }
        public int SyncIntervalSeconds { get; }
        public string LogFilePath { get; }

        public SyncConfig(string[] args)
        {
            SourcePath = args[0];
            ReplicaPath = args[1];
            LogFilePath = args[3];

            int.TryParse(args[2], out var interval);
            SyncIntervalSeconds = interval;
        }

        public bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(SourcePath))
            {
                Console.WriteLine("You didn't put a path in source.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(ReplicaPath))
            {
                Console.WriteLine("You didn't put a path in replica.");
                return false;
            }
            if (SyncIntervalSeconds <= 0)
            {
                Console.WriteLine("Invalid synchronization interval.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(LogFilePath))
            {
                Console.WriteLine("You didn't put a path for log file.");
                return false;
            }
            return true;
        }
    }

}
