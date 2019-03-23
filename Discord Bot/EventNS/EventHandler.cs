using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord_Bot.CommandNS;
using Discord_Bot.GuildNS;
using Discord_Bot.LoggerNS;

namespace Discord_Bot.EventNS
{
    public class EventHandler
    {
        private DiscordBot _discordBot;
        
        public EventHandler(DiscordBot discordBot)
        {
            _discordBot = discordBot;
            
            SetupMethods();
        }

        public Task DiscordNetLog(LogMessage logMessage)
        {
            DiscordBot.Logger.Log(Level.LogSeverityToLevel(logMessage.Severity), logMessage.Message, logMessage.Exception);
            return Task.CompletedTask;
        }
        
        public Task DiscordMessageRecieved(SocketMessage message)
        {
            if (message.Content.StartsWith(Executor.CommandPrefix.ToString()))
            {
                string wholeCommand = message.Content;
                List<string> args = wholeCommand.Substring(1).Split(' ').ToList();
                string command = args[0];
                args.RemoveAt(0);
                
                _discordBot.CommandExecutor.OnCommand(message, command, args.ToArray());
            }


            if (message.Author.GetType() == typeof(SocketGuildUser))
            {
                SocketGuildUser guildUser = (SocketGuildUser) message.Author;
                if (!guildUser.IsBot)
                {
                    Guild guild = _discordBot.GuildManager.GetGuild(guildUser.Guild);

                    GuildRankManager rankManager = guild.RankManager;

                    SocketRole oldRank = rankManager.GetRank(guild.RankManager.GetUserPoints(guildUser.Id));

                    rankManager.SetRolePoints(guildUser.Id,
                        guild.RankManager.GetUserPoints(guildUser.Id) + (uint) message.Content.Length);

                    SocketRole newRank = rankManager.GetRank(guild.RankManager.GetUserPoints(guildUser.Id));

                    if (oldRank.Id != newRank.Id)
                    {
                        message.Channel.SendMessageAsync($"<@{message.Author.Id}> has been ranked up to {newRank.Name}!");
                        guildUser.AddRoleAsync(newRank);
                        guildUser.RemoveRoleAsync(oldRank);
                    }
                }

            }
            
            return Task.CompletedTask;
        }


        public Task GuildAvailable(SocketGuild socketGuild)
        {
            DiscordBot.Logger.Log(Level.Verbose, $"Guild \"{socketGuild.Name}\" is available.");
            _discordBot.GuildManager.AddGuild(socketGuild);
            return Task.CompletedTask;
        }
        
        
        private void SetupMethods()
        {
            _discordBot.client.Log += DiscordNetLog;
            _discordBot.client.MessageReceived += DiscordMessageRecieved;
            _discordBot.client.GuildAvailable += GuildAvailable;
        }
    }
}