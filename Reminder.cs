namespace DiscordBot
{
    class Reminder
    {
        public static void AddMessageDate()
        {
            System.IO.StreamWriter sw = System.IO.File.AppendText(
                Logger.GetTempPath() + "DiscordBotMessageDateLog.txt");
            try
            {
                string logLine = System.String.Format(
                    "{0:G}", System.DateTime.Now);
                sw.WriteLine(logLine);
            }
            finally
            {
                sw.Close();
            }
        }
    }
}