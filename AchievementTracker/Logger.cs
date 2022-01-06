using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AchievementTracker
{
    public static class Logger
    {
        public static void Log(string msg)
        {
            AchievementTracker.Instance.ModHelper.Console.WriteLine("Log: " + msg, OWML.Common.MessageType.Info);
        }

        public static void LogError(string msg)
        {
            AchievementTracker.Instance.ModHelper.Console.WriteLine("Error: " + msg, OWML.Common.MessageType.Error);
        }
    }
}
