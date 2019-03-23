using System.IO;
using Discord.WebSocket;
using Discord_Bot.LoggerNS;
using Newtonsoft.Json.Linq;

namespace Discord_Bot
{
    public class GuildFile
    {
        private readonly string _guildFileLoc;

        protected internal readonly SocketGuild Guild;
        protected internal readonly JObject ObjGuildFile;

        private const string FileTemplate =
            "{\n" +
              "  \"nsfw_role\": \"\",\n" +
              "  \"ranks\": {\n" +
              "  },\n" +
              "  \"users\": {\n" +
              "  }\n" +
            "}";
        
        protected internal SocketRole NsfwRole
        {
            get
            {
                string idStr = (string) ObjGuildFile["nsfw_role"];
                if (idStr != "")
                {
                    ulong id = ulong.Parse(idStr);
                    return Guild.GetRole(id);
                }
                return null;         
            }
            set
            {
                if (ObjGuildFile.ContainsKey("nsfw-role"))
                {
                    ObjGuildFile["nsfw_role"] = value.ToString();
                }
                else
                {
                    ObjGuildFile.Add("nsfw_role", value.ToString());
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

            Guild = socketGuild;
            _guildFileLoc = guildFileLoc;    
            ObjGuildFile = JObject.Parse(File.ReadAllText(_guildFileLoc));
        }

        protected internal void Save()
        {
            // TODO might be wrong saving this ~ might be appending to the file
            if (!File.Exists(_guildFileLoc))
            {
                WriteGuildTemplate(_guildFileLoc);
            }
            File.WriteAllText(_guildFileLoc, ObjGuildFile.ToString());
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