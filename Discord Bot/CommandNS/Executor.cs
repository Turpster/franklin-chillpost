using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Reflection.Emit;
using Discord.WebSocket;
using Discord_Bot.CommandNS.CommandsNS;
using Discord_Bot.LoggerNS;

namespace Discord_Bot.CommandNS
{
    public class Executor
    {
        public const char CommandPrefix = '!';
        
        private readonly Dictionary<string[], Command> _executors = new Dictionary<string[], Command>();

        protected internal DiscordBot DiscordBot;

        public Executor(DiscordBot discordBot)
        {
            DiscordBot = discordBot;
            AddCommands();
            
        }

        public void AddCommandExecutor(string[] labels, Command command)
        {
            foreach (string targetLabel in labels)
            {
                if (GetCommandAssociation(targetLabel) != null)
                {
                    throw new DataException("Executor label has already been added to the CommandHandler.");  
                }
            }
            
            _executors.Add(labels, command);
        }

        public void OnCommand(SocketMessage socketMessage, string label, string[] args)
        {
            Command targetCommand = GetCommandAssociation(label);
            if (targetCommand != null)
            {
                targetCommand.OnCommand(socketMessage, args);
            }

            else socketMessage.Channel.SendMessageAsync($"<@{socketMessage.Author.Id}> {label} is an invalid command.");
        }

        private Command GetCommandAssociation(string label)
        {
            foreach (KeyValuePair<string[], Command> executor in _executors)
            {
                foreach (string commandLabel in executor.Key)
                {
                    if (label.Equals(commandLabel, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return executor.Value;
                    }
                }
            }

            return null;
        }

        private void AddCommands()
        {
            new Ping(this);
            new NSFW(this);
        }
    }
}