namespace DiscordBot
{
    class Logger
    {
        public static string GetTempPath()
        {
            if (settings.get_logFolder() == null)
            {
                string path = System.Environment.GetEnvironmentVariable("TEMP");
                if (!path.EndsWith("\\")) path += "\\";
                return path;
            } else
            {
                return settings.get_logFolder();
            }
        }

        public static void LogMessageToFile(string msg)
        {
            System.IO.StreamWriter sw = System.IO.File.AppendText(
                GetTempPath() + "DiscordBotLog.txt");
            try
            {
                string logLine = System.String.Format(
                    "{0:G}: {1}.", System.DateTime.Now, msg);
                sw.WriteLine(logLine);
            }
            finally
            {
                sw.Close();
            }
        }
    }
}
