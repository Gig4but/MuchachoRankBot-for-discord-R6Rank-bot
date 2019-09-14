using Discord;
using Discord.Commands;
using System;
using System.Linq;
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
        public async Task getRank(string nick, string platform)
        {
            switch (platform.ToLower())
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
                // create the url -> parse the site -> check if site was parsed -> nick was correct
                string url = "https://r6tab.com/api/search.php?platform=" + platform + "&search=" + nick;
                string source = await Parser.getSite(url);

                if (source == null)
                    await ReplyAsync("errorDownload:nullSourceInfo");

                string test = source.Substring(source.IndexOf("totalresults") + 14, 1);
                if (test != "0")
                {
                    // take the ubisoft player ID -> create the url -> parse the site -> check if site was parsed -> check if nick was correct
                    source = source.Substring(source.IndexOf("p_id") + 7, 36);
                    url = "https://r6tab.com/api/player.php?p_id=" + source;
                    source = await Parser.getSite(url);
                    if (source != null)
                    {
                        /* take the number in string -> delete the comma from string if exist -> 
                        -> convert number from string to int -> set the rank name from rank[number] ->
                        -> send the built msg to chat */
                        string p_currentrank = staff.ranks[Int32.Parse(staff.regex(source.Substring(source.IndexOf("p_currentrank") + 15, 2)))];
                        string p_maxrank = staff.ranks[Int32.Parse(staff.regex(source.Substring(source.IndexOf("p_maxrank") + 11, 2)))];
                        await Context.Channel.SendMessageAsync("", false, staff.msgBuilder(p_currentrank, p_maxrank));

                        // create user and role socket -> remove old role -> add new role -> log changes
                        Discord.WebSocket.SocketUser user = Context.User;
                        Discord.WebSocket.SocketRole role = null;
                        for (int i = 0; i < staff.ranks.Length; i++)
                        {
                            role = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == staff.ranks[i]);
                            await (user as IGuildUser).RemoveRoleAsync(role);
                        }
                        role = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == p_currentrank);
                        await (user as IGuildUser).AddRoleAsync(role);
                        Logger.LogMessageToFile(user.Username + " get role/rank " + p_currentrank);
                    }
                    else
                        await ReplyAsync("errorDownload:nullSourceInfo");
                }
                else
                    await ReplyAsync("Please, type the correct user nickname");
            }
            else
                await ReplyAsync("Please, type the correct platform name");
        }
        [Command("rank")]
        public async Task getRankS(string nick)
        {
            // create the url -> parse the site -> check if site was parsed -> nick was correct
            string url = "https://r6tab.com/api/search.php?platform=uplay&search=" + nick;
            string source = await Parser.getSite(url);

            if (source == null)
                await ReplyAsync("errorDownload:nullSourceInfo");

            string test = source.Substring(source.IndexOf("totalresults") + 14, 1);
            if (test != "0")
            {
                // take the ubisoft player ID -> create the url -> parse the site -> check if site was parsed -> check if nick was correct
                source = source.Substring(source.IndexOf("p_id") + 7, 36);
                url = "https://r6tab.com/api/player.php?p_id=" + source;
                source = await Parser.getSite(url);
                if (source != null) {
                    /* take the number in string -> delete the comma from string if exist -> 
                    -> convert number from string to int -> set the rank name from rank[number] ->
                    -> send the built msg to chat */
                    string p_currentrank = staff.ranks[Int32.Parse( staff.regex(source.Substring(source.IndexOf("p_currentrank") + 15, 2)) )];
                    string p_maxrank = staff.ranks[Int32.Parse( staff.regex(source.Substring(source.IndexOf("p_maxrank") + 11, 2)) )];
                    await Context.Channel.SendMessageAsync("", false, staff.msgBuilder(p_currentrank, p_maxrank));

                    // create user and role socket -> remove old role -> add new role -> log changes
                    Discord.WebSocket.SocketUser user = Context.User;
                    Discord.WebSocket.SocketRole role = null;
                    for (int i = 0; i < staff.ranks.Length; i++)
                    {
                        role = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == staff.ranks[i]);
                        await (user as IGuildUser).RemoveRoleAsync(role);
                    }
                    role = Context.Guild.Roles.FirstOrDefault(x => x.Name.ToString() == p_currentrank);
                    await (user as IGuildUser).AddRoleAsync(role);
                    Logger.LogMessageToFile(user.Username + " get role/rank " + p_currentrank);
                }
                else
                    await ReplyAsync("errorDownload:nullSourceInfo");
            }
            else
                await ReplyAsync("Please, type the correct user nickname");
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
