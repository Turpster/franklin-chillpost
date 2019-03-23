using System;
using System.Linq;
using Discord.Rest;
using Discord.WebSocket;
using Discord_Bot.GuildNS;

namespace Discord_Bot.CommandNS.CommandsNS
{
    public class NSFW : Command
    {
        public NSFW(Executor executor) : base(new [] {"nsfw", "porn"}, executor) {}

        public override void OnCommand(SocketMessage rawSocketMessage, string[] args)
        {
            if (rawSocketMessage.Author.GetType() == typeof(SocketGuildUser))
            {
                if (args.Contains("confirm", StringComparer.InvariantCultureIgnoreCase))
                {
                    SocketGuildUser socketGuildUser = (SocketGuildUser) rawSocketMessage.Author;

                    Guild guild = Executor.DiscordBot.GuildManager.GetGuild(socketGuildUser.Guild);

                    bool hasRole = false;


                    if (guild.NsfwRole != null)
                    {
                        //Check if user is in NSFW role
                        foreach (SocketRole role in socketGuildUser.Roles)
                        {
                            if (role.Id == guild.NsfwRole.Id)
                            {
                                hasRole = true;
                            }
                        }

                        if (!hasRole)
                        {
                            rawSocketMessage.Channel.SendMessageAsync(
                                $"<@{rawSocketMessage.Author.Id}> You are now part of the NSFW role.");
                            socketGuildUser.AddRoleAsync(guild.NsfwRole);


                        }
                        else
                        {
                            rawSocketMessage.Channel.SendMessageAsync(
                                $"<@{rawSocketMessage.Author.Id}> You are already in the NSFW role.");
                        }
                    }
                    else
                    {
                        rawSocketMessage.Channel.SendMessageAsync(
                            $"<@{rawSocketMessage.Author.Id}> Please contact your administrator and tell him to add an NSFW role to the server file");
                    }
                }
                else
                {
                rawSocketMessage.Channel.SendMessageAsync(
                    $"<@{rawSocketMessage.Author.Id}> NSFW (not-safe-for-work) includes adult material and should not be used by anyone under the age of 18.\n" +
                    $"Please type \"{Executor.CommandPrefix}nsfw confirm\" to confirm this disclaimer.");
                }

            }
        }
    }
}