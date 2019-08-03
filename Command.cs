using Discord;
using Discord.Commands;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        [Command("MuchoRank")]
        public async Task info()
        {
            await ReplyAsync("!rank NICK PLATFORM[PC, XBOX, PS4]");
        }
        [Command("rank")]
        public async Task rank(string nick, string platform)
        {
            platform = platform.ToLower();
            switch (platform)
            {
                case "pc":
                    platform = "uplay";
                    break;
                case "xbox":
                    platform = "xbl";
                    break;
                case "ps4":
                    platform = "psn";
                    break;
                default:
                    platform = null;
                    break;
            }
            if (platform != null)
            {
                string url = "https://r6tab.com/api/search.php?platform=" + platform + "&search=" + nick;
                HttpClient client = new HttpClient();
                var response = await client.GetAsync(url);
                string source = null;
                if (response != null && response.StatusCode == HttpStatusCode.OK)
                {
                    source = await response.Content.ReadAsStringAsync();
                }
                string subStringResult = "totalresults";
                string test = source.Substring(source.IndexOf(subStringResult) + 14, 1);
                if (Int32.Parse(test) != 0)
                {
                    string subStringId = "p_id";
                    source = source.Substring(source.IndexOf(subStringId) + 7, 36);
                    url = "https://r6tab.com/api/player.php?p_id=" + source;
                    client = new HttpClient();
                    response = await client.GetAsync(url);
                    source = null;
                    if (response != null && response.StatusCode == HttpStatusCode.OK)
                    {
                        source = await response.Content.ReadAsStringAsync();
                    }
                    string subStringCurRank = "p_currentrank";
                    string subStringMaxRank = "p_maxrank";
                    string p_currentrank = null;
                    string p_maxrank = null;
                    p_currentrank = source.Substring(source.IndexOf(subStringCurRank) + 15, 2);
                    p_maxrank = source.Substring(source.IndexOf(subStringMaxRank) + 11, 2);
                    string[] ranks = {"unrank",
                            "Copper 4","Copper 3","Copper 2","Copper 1",
                            "Bronze 4", "Bronze 3", "Bronze 2", "Bronze 1",
                            "Silver 4", "Silver 3", "Silver 2", "Silver 1",
                            "Gold 4", "Gold 3", "Gold 2", "Gold 1",
                            "Platinum 3", "Platinum 2", "Platinum 1", "Diamond"};
                    if(Regex.IsMatch(p_currentrank.Substring(1), ","))
                    {
                        p_currentrank = p_currentrank.Substring(0, 1);
                    }
                    if (Regex.IsMatch(p_maxrank.Substring(1), ","))
                    {
                        p_maxrank = p_maxrank.Substring(0, 1);
                    }
                    if (source != null)
                    {
                        string resultF = ranks[Int32.Parse(p_currentrank)];
                        string result = ranks[Int32.Parse(p_maxrank)];
                        var Builder = new EmbedBuilder();
                        Builder.WithDescription("Your current rank is " + resultF + " \n" +
                            "Your max rank is " + result + "\n New(or not new) role " + result);
                        if (Regex.IsMatch(result.Substring(0), "C"))
                            Builder.WithColor(0x995500);
                        else if (Regex.IsMatch(result.Substring(0), "B"))
                            Builder.WithColor(0xff9600);
                        else if (Regex.IsMatch(result.Substring(0), "S"))
                            Builder.WithColor(0x8c8c8c);
                        else if (Regex.IsMatch(result.Substring(0), "G"))
                            Builder.WithColor(0xffff00);
                        else if (Regex.IsMatch(result.Substring(0), "P"))
                            Builder.WithColor(0x00ffff);
                        else if (Regex.IsMatch(result.Substring(0), "D"))
                            Builder.WithColor(0x9900ff);
                        await Context.Channel.SendMessageAsync("", false, Builder.Build());
                        var user = Context.User;
                        var role = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == result);
                        for (int i = 0; i < ranks.Length; i++)
                        {
                            role = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == ranks[i]);
                            await (user as IGuildUser).RemoveRoleAsync(role);
                        }
                        role = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == result);
                        await (user as IGuildUser).AddRoleAsync(role);
                        Logger.LogMessageToFile(user.Username+" get role/rank "+ result);
                    }
                    else
                    {
                        await ReplyAsync("ERR:nullSourceInfo");
                    }
                } else
                {
                    await ReplyAsync("Please, type the correct user nickname");
                }
            }
            else
            {
                await ReplyAsync("Please, type the correct platform name");
            }
        }
        [Command("settings")]
        public async Task settingsCommands(string password)
        {
            if (settings.get_settingsPassword() == password) {
                await Discord.UserExtensions.SendMessageAsync(Context.Message.Author, "!settings pass \nsetPass\nsetStatus\nsetLogFolder");
            } else
            {
                await Discord.UserExtensions.SendMessageAsync(Context.Message.Author, "Wrong password");
            }
        }
        [Command("settings")]
        public async Task settingsInfo(string password, string command)
        {
            if (settings.get_settingsPassword() == password) {
                switch (command) {
                    case "setPass":
                        await Discord.UserExtensions.SendMessageAsync(Context.Message.Author, "!settings pass setPass TEXT ===> " +
                            "changes password to settings acess. \nDefault: [1234]");
                        break;
                    case "setStatus":
                        await Discord.UserExtensions.SendMessageAsync(Context.Message.Author, "!settings pass setStatus TEXT ===> " +
                            "changes bot status. \nDefault: [!rank]");
                        break;
                    case "setLogFolder":
                        await Discord.UserExtensions.SendMessageAsync(Context.Message.Author, "!settings pass setLogFolder TEXT ===> " +
                            "changes save folder for log. \nDefault: [...\\TEMP\\DiscordBotLog.txt] " +
                            "\nPlease, type paths only: \n\ta) in english symbols in format \n\tb) in format [DISK:\\...\\]");
                        break;
                    default:
                        await Discord.UserExtensions.SendMessageAsync(Context.Message.Author, "Wrong command name");
                        break;
                }
                
            } else
            {
                await Discord.UserExtensions.SendMessageAsync(Context.Message.Author, "Wrong password");
            }
        }
        [Command("settings")]
        public async Task settingsAction(string password, string action, string txt)
        {
            if (settings.get_settingsPassword() == password) {
                switch (action)
                {
                    case "setPass":
                        settings.set_settingsPassword(txt);
                        Logger.LogMessageToFile(Context.User.Username+" set settings password to "+txt);
                        break;
                    case "setStatus":
                        settings.set_botStatus(txt);
                        Logger.LogMessageToFile(Context.User.Username + " set bot status to " + txt);
                        break;
                    case "setLogFolder":

                        break;
                    default:
                        await Discord.UserExtensions.SendMessageAsync(Context.Message.Author, "Wrong action name");
                        break;
                }
            } else
            {
                await Discord.UserExtensions.SendMessageAsync(Context.Message.Author, "Wrong password");
            }
        }
    }
}
