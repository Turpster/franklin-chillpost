using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using Discord.WebSocket;
using Discord_Bot.LoggerNS;
using Newtonsoft.Json.Linq;

namespace Discord_Bot
{
    public class GuildFile
    {
        private readonly string _guildFileLoc;

        SocketGuild _guild;
        private readonly JObject _rolesFile;

        private const string FileTemplate =
            "{\n" +
            "  \"nsfw_role\": \"\"\n" +
            "}";
        
        protected internal SocketRole NsfwRole
        {
            get
            {
                string idStr = (string) _rolesFile["nsfw_role"];
                if (idStr != "")
                {
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
                WriteGuildTemplate(guildFileLoc);
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
                WriteGuildTemplate(_guildFileLoc);
            }
            File.WriteAllText(_guildFileLoc, _rolesFile.ToString());
        }
        
        private void WriteGuildTemplate(string fileLoc)
        {
            DiscordBot.Logger.Log(Level.Verbose, $"Creating Guild file at {fileLoc}");
            StreamWriter writer = File.CreateText(fileLoc);
            writer.Write(FileTemplate);
            writer.Close();
        }
    }
}