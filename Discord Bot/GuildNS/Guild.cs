using System.IO;
using Discord.WebSocket;
using Discord_Bot.LoggerNS;

namespace Discord_Bot.GuildNS
{
    public class Guild
    {
        protected internal readonly SocketGuild _socketGuild;
        public const string GuildFileDirectory = @"./servers/";

        private GuildFile _guildFile;
        public GuildRankManager RankManager;

        public SocketRole NsfwRole
        {
            get => _guildFile.NsfwRole;
            set => _guildFile.NsfwRole = value;
        }
        
        public Guild(SocketGuild socketGuild)
        {
            if (!Directory.Exists(GuildFileDirectory))
            {
                DiscordBot.Logger.Log(Level.Verbose, $"Creating Guild file at {GuildFileDirectory}");
                Directory.CreateDirectory(GuildFileDirectory);
            }
            
            _socketGuild = socketGuild;
            _guildFile = new GuildFile(_socketGuild, GuildFileDirectory + _socketGuild.Id + ".json");
            RankManager = new GuildRankManager(_guildFile);
        }
    }
}