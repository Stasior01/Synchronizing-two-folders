using System;
using Serilog;

namespace Synchronizing_two_folders.Services
{
    public class MenuService
    {
        private readonly Action forceSyncAction;
        private readonly Action<int> changeIntervalAction;
        private readonly Action exitAction;

        public MenuService(Action forceSync, Action<int> changeInterval, Action exit)
        {
            forceSyncAction = forceSync;
            changeIntervalAction = changeInterval;
            exitAction = exit;
        }

        public void ShowMenu()
        {
            Console.WriteLine("\n[MENU] Press:");
            Console.WriteLine("1 - Synchronize now");
            Console.WriteLine("I - Change interval");
            Console.WriteLine("ESC - Exit");

            var key = Console.ReadKey(true);

            switch (key.Key)
            {
                case ConsoleKey.D1:
                case ConsoleKey.NumPad1:
                    forceSyncAction.Invoke();
                    Log.Information("Manual sync requested.");
                    break;

                case ConsoleKey.I:
                    Console.Write("Enter new interval in seconds: ");
                    if (int.TryParse(Console.ReadLine(), out int newInterval) && newInterval > 0)
                        changeIntervalAction.Invoke(newInterval);
                    else
                        Console.WriteLine("Invalid value.");
                    break;

                case ConsoleKey.Escape:
                    exitAction.Invoke();
                    break;
            }
        }
    }
}
