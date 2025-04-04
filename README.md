# Folder Synchronizer (C# and .NET)

A console application that synchronizes two folders: a SOURCE and a REPLICA. The goal is to keep the replica an exact, up-to-date copy of the source folder, with automatic file creation, updates, and deletion.

---

## How to run

Option 1:

Option 1 – **Command Line Arguments**:

bash
Synchronizing_two_folders.exe "C:\Source" "C:\Replica" 10 "C:\Logs"

C:\Source – path to the source folder
C:\Replica – path to the replica folder
10 – synchronization interval in seconds
C:\Logs – folder where the log file synchronization.log will be created (or updated if exist)

Option 2:

Interactive Mode (No Arguments)

If no arguments are provided, the program will prompt the user to input:
-Source folder path
-Replica folder path
-Synchronization interval (seconds)
-Log folder path

The paths are validated to ensure they exist and are accessible.

---

This project is provided for recruitment purposes.

---

AUTHOR
Krystian Stasik