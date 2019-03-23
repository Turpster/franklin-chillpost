using System.IO;
using System.Net;
using Discord.WebSocket;
using Discord_Bot.LoggerNS;
using Newtonsoft.Json.Linq;

namespace Discord_Bot.RolesNS
{
    public class GuildFile
    {
        private readonly string _guildFileLoc;

        SocketGuild _guild;
        private readonly JObject _rolesFile;
        
        protected internal SocketRole NsfwRole
        {
            get
            {
                if (_rolesFile.ContainsKey("nsfw_role"))
                {
                    string idStr = (string) _rolesFile["nsfw_role"];
                    ulong id = ulong.Parse(idStr);
                    return _guild.GetRole(id);
                }
                return null;         
            }
            set
            {
                if (_rolesFile.ContainsKey("nsfw-role"))
                {
                    _rolesFile["nsfw_role"] = value.ToString();
                }
                else
                {
                    _rolesFile.Add("nsfw_role", value.ToString());
                }

                Save();
            }
        }

        public GuildFile(SocketGuild socketGuild, string guildFileLoc)
        {
            if (!File.Exists(guildFileLoc))
            {
                DiscordBot.Logger.Log(Level.Verbose, $"Creating Guild file at {guildFileLoc}");
                StreamWriter writer = File.CreateText(guildFileLoc);
                writer.Write("{}");
                writer.Close();
            }

            _guild = socketGuild;
            _guildFileLoc = guildFileLoc;    
            _rolesFile = JObject.Parse(File.ReadAllText(_guildFileLoc));
        }

        private void Save()
        {
            // TODO might be wrong saving this ~ might be appending to the file
            if (!File.Exists(_guildFileLoc))
            {
                DiscordBot.Logger.Log(Level.Verbose, $"Creating Guild file at {_guildFileLoc}");
                StreamWriter writer = File.CreateText(_guildFileLoc);
                writer.Write("{}");
                writer.Close();
            }
            File.WriteAllText(_guildFileLoc, _rolesFile.ToString());
        }
    }
}