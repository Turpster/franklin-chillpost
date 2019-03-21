using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord_Bot.LoggerNS;

namespace Discord_Bot
{
    public class DiscordBot
    {
        public BaseSocketClient client;

        public static readonly Logger Logger = new Logger();
        
        public static void Main(string[] args)
        {
            string token = "";

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Equals("-t", StringComparison.CurrentCulture))
                {
                    token = args[i + 1];
                }
            }

            if (token == "")
            {
                Console.Write("Please enter token: ");
                token = Console.ReadLine();
            }

            DiscordBot bot = new DiscordBot();
            bot.Login(token).GetAwaiter().GetResult();
        }

        public DiscordBot()
        {
            client = new DiscordSocketClient();
            
            client.Log += logMessage =>
            {
                Logger.Log(Level.LogSeverityToLevel(logMessage.Severity), logMessage.Message, logMessage.Exception);
                return Task.CompletedTask;
            };
            
        }

        private async Task Login(string token)
        {
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            await Task.Delay(-1);
        }
    }
}