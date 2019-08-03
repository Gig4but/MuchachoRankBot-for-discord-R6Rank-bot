namespace DiscordBot
{
    class settings
    {
        private static string settingsPassword = "1234";
        private static string botStatus = "!MuchoRank";
        private static string logFolder = null;

        public static string get_settingsPassword()
        {
            return settingsPassword;
        }
        public static string get_botStatus()
        {
            return botStatus;
        }
        public static string get_logFolder()
        {
            return logFolder;
        }

        public static void set_settingsPassword(string txt)
        {
            settingsPassword = txt;
        }
        public static void set_botStatus(string txt)
        {
            botStatus = txt;
        }
        public static void set_logFolder(string txt)
        {
            logFolder = txt;
        }
    }
}
