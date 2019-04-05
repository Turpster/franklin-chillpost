using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord_Bot.CommandNS;
using Discord_Bot.GuildNS;
using Discord_Bot.LoggerNS;
using EventHandler = Discord_Bot.EventNS.EventHandler;
using TaskScheduler = Discord_Bot.SchedulerNS.TaskScheduler;

namespace Discord_Bot
{
    public class DiscordBot
    {
        public BaseSocketClient client;
        public Executor CommandExecutor;
        public EventHandler EventHandler;
        public GuildManager GuildManager;

        public TaskScheduler Scheduler;

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

            AppDomain.CurrentDomain.UnhandledException += (sender, eventArgs) =>
            {
                Logger.Log(Level.Error, "An error has occured", (Exception) eventArgs.ExceptionObject);
            };

            DiscordBot bot = new DiscordBot();
            bot.Login(token).GetAwaiter().GetResult();
        }

        public DiscordBot()
        {
            Scheduler = new TaskScheduler();
            GuildManager = new GuildManager();
            client = new DiscordSocketClient();
            CommandExecutor = new Executor(this);
            EventHandler = new EventHandler(this);
        }

        private async Task Login(string token)
        {
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            await Task.Delay(-1);
        }
    }
}