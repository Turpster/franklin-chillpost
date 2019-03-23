using System.Collections.Generic;
using System.Data;
using Discord.WebSocket;

namespace Discord_Bot.GuildNS
{
    public class GuildManager
    {
        private List<Guild> _guilds = new List<Guild>();
        public Guild[] Guilds => _guilds.ToArray();

        public void AddGuild(SocketGuild socketGuild)
        {
            if (!ContainsGuild(socketGuild.Id))
            {
                _guilds.Add(new Guild(socketGuild));
            } 
            else throw new DataException("Guild has already been added to the list of servers in the Guild Manager.");
        }

        public bool ContainsGuild(ulong guildId)
        {
            foreach (Guild guild in Guilds)
            {
                if (guild._socketGuild.Id == guildId)
                {
                    return true;
                }
            }

            return false;
        }
        
        public Guild GetGuild(SocketGuild targetGuild)
        {
            foreach (Guild guild in _guilds)
            {
                if (guild._socketGuild.Id == targetGuild.Id)
                {
                    return guild;
                }
            }

            return null;
        }
    }
}