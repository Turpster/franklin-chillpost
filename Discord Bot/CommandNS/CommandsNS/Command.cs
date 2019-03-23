using Discord.WebSocket;
using Discord_Bot.LoggerNS;

namespace Discord_Bot.CommandNS
{
    public abstract class Command
    {
        protected Executor Executor;
        
        internal Command(string[] commandLabels, Executor executor)
        {
            string aliases = "";

            foreach (var commandLabel in commandLabels)
            {
                aliases += commandLabel + ", ";
            }

            aliases = aliases.Substring(0, aliases.Length - 2);
            
            DiscordBot.Logger.Log(Level.Verbose, "Added commands: " + LoggerFormat.Bold + LoggerFormat.LightCyan + aliases + LoggerFormat.Reset + ".");
            executor.AddCommandExecutor(commandLabels, this);
            Executor = executor;
        }
        
        public abstract void OnCommand(SocketMessage rawSocketMessage, string[] args);
    }
}