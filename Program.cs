using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace DiscordBot
{
    class Bot
    {
        static void Main(string[] args) => new Bot().RunBotAsync().GetAwaiter().GetResult();
        private DiscordSocketClient client;
        private CommandService commands;
        private IServiceProvider services;
        public async Task RunBotAsync()
        {
            client = new DiscordSocketClient();
            commands = new CommandService();
            services = new ServiceCollection()
                .AddSingleton(client)
                .AddSingleton(commands)
                .BuildServiceProvider();
            string botToken = "NjA2MDY5MDMxMDYwODk3ODIy.XWU5kg.nHRiMZkvvAotOqLXlZ7-04-REPo";
            client.Log += Log;
            await RegisterCommandsAsync();
            await client.LoginAsync(Discord.TokenType.Bot, botToken);
            await client.StartAsync();
            await client.SetGameAsync(settings.get_botStatus());
            await Task.Delay(-1);
        }
        private Task Log(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }
        public async Task RegisterCommandsAsync()
        {
            client.MessageReceived += HandleCommandAsync;
            await commands.AddModulesAsync(Assembly.GetEntryAssembly(), services);
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            if (message is null || message.Author.IsBot) return;
            int argPos = 0;
            if (message.HasStringPrefix("!", ref argPos) || message.HasMentionPrefix(client.CurrentUser, ref argPos))
            {
                var context = new SocketCommandContext(client, message);
                var result = await commands.ExecuteAsync(context, argPos, services);
                if (!result.IsSuccess)
                    Console.WriteLine(result.ErrorReason);
            }
            await client.SetGameAsync(settings.get_botStatus());
        }
    }
}
