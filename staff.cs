using Discord;
using System.Text.RegularExpressions;

namespace DiscordBot
{
    class staff
    {
        public static string[] ranks = {"unrank",
        "Copper 4","Copper 3","Copper 2","Copper 1",
        "Bronze 4", "Bronze 3", "Bronze 2", "Bronze 1",
        "Silver 4", "Silver 3", "Silver 2", "Silver 1",
        "Gold 4", "Gold 3", "Gold 2", "Gold 1",
        "Platinum 3", "Platinum 2", "Platinum 1", "Diamond"};

        public static string regex(string txt)
        {
            if (Regex.IsMatch(txt.Substring(1), ","))
            {
                txt = txt.Substring(0, 1);
            }
            return txt;
        }

        public static Embed msgBuilder(string p_currentrank, string p_maxrank)
        {
            EmbedBuilder Builder = new EmbedBuilder();
            Builder.WithDescription("Your current rank is " + p_currentrank + " \n" +
                "Your max rank is " + p_maxrank /*+ "\n New(or not new) role " + result*/);

            p_currentrank = p_currentrank.Substring(0);
            if (Regex.IsMatch(p_currentrank, "C"))
                Builder.WithColor(0x995500);
            else if (Regex.IsMatch(p_currentrank, "B"))
                Builder.WithColor(0xff9600);
            else if (Regex.IsMatch(p_currentrank, "S"))
                Builder.WithColor(0x8c8c8c);
            else if (Regex.IsMatch(p_currentrank, "G"))
                Builder.WithColor(0xffff00);
            else if (Regex.IsMatch(p_currentrank, "P"))
                Builder.WithColor(0x00ffff);
            else if (Regex.IsMatch(p_currentrank, "D"))
                Builder.WithColor(0x9900ff);

            return Builder.Build();
        }
    }
}
