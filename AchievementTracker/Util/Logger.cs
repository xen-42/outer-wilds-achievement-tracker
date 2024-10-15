namespace AchievementTracker.Util
{
    public static class Logger
    {
        public static void Log(string msg)
        {
            if (Main.Instance == null || !Main.ShowMoreLogs) return;

            Main.Instance.ModHelper.Console.WriteLine("[Achievements] Log: " + msg, OWML.Common.MessageType.Info);
        }

        public static void LogError(string msg)
        {
            if (Main.Instance == null) return;

            Main.Instance.ModHelper.Console.WriteLine("[Achievements] Error: " + msg, OWML.Common.MessageType.Error);
        }

        public static void LogWarning(string msg)
        {
            if (Main.Instance == null || !Main.ShowMoreLogs) return;

            Main.Instance.ModHelper.Console.WriteLine("[Achievements] Warning: " + msg, OWML.Common.MessageType.Warning);
        }
    }
}
