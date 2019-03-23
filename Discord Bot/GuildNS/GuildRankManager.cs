using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Data;
using Discord.WebSocket;
using Discord_Bot.LoggerNS;
using Newtonsoft.Json.Linq;

namespace Discord_Bot
{
    public class GuildRankManager
    {
        private SocketGuild _socketGuild;
        private GuildFile _guildFile;
        
        private KeyValuePair<SocketRole, uint>[] Ranks
        {
            get
            {
                JProperty[] rankTokens = ((JContainer) _guildFile.ObjGuildFile["ranks"]).Values<JProperty>().ToArray();
                    
                KeyValuePair<SocketRole, uint>[] ranks = new KeyValuePair<SocketRole, uint>[rankTokens.Length];

                for (int i = 0; i < rankTokens.Length; i++)
                {
                    ranks[i] = new KeyValuePair<SocketRole, uint>(_socketGuild.GetRole(ulong.Parse(rankTokens[i].Name)), uint.Parse(rankTokens[i].Value.ToString())); 
                }

                return ranks;
            }
        }

        public void SetRankPoints(ulong roleId, uint points)
        {
            if (_guildFile.ObjGuildFile["ranks"].Contains(roleId.ToString()))
            {
                _guildFile.ObjGuildFile["ranks"][roleId.ToString()] = points.ToString();
            }
            else
            {
                JContainer container = (JContainer) _guildFile.ObjGuildFile["ranks"];
                container.Add(new JProperty(roleId.ToString(), points.ToString()));
                _guildFile.ObjGuildFile["ranks"] = container;
            }
            _guildFile.Save();
        }
        
        public void SetRolePoints(ulong guildUserId, uint points)
        {
            JProperty[] properties = ((JContainer) _guildFile.ObjGuildFile["users"]).Values<JProperty>().ToArray();

            bool isUserInFile = false;

            foreach (JProperty property in properties)
            {
                if (property.Name == guildUserId.ToString())
                {
                    isUserInFile = true;
                    break;
                }
            }
            
            if (isUserInFile)
            {
                _guildFile.ObjGuildFile["users"][guildUserId.ToString()] = points.ToString();
            }
            else
            {
                JContainer container = (JContainer) _guildFile.ObjGuildFile["users"];
                container.Add(new JProperty(guildUserId.ToString(), points.ToString()));
                _guildFile.ObjGuildFile["users"] = container;
            }
            _guildFile.Save();
        }
        
        public GuildRankManager(GuildFile guildFile)
        {
            _guildFile = guildFile;
            _socketGuild = guildFile.Guild;
        }

        public uint GetUserPoints(ulong userId)
        {
            JContainer container = (JContainer) _guildFile.ObjGuildFile["users"];
            
            if (container[userId.ToString()] != null)
            {
                return uint.Parse((string) container[userId.ToString()]);
            }
            container.Add(new JProperty(userId.ToString(), 0));
            _guildFile.ObjGuildFile["users"] = container;
            _guildFile.Save();
            return 0;
        }

        public SocketRole GetRank(uint points)
        {
            for (int i = Ranks.Length - 1; i >= 0; i--)
            {
                var Rank = Ranks[i];
                if (Rank.Value <= points)
                {
                    return Rank.Key;
                }
                
            }
            
            throw new DataException($"Invalid points ~ points: {points} is too small for the lowest rank");
        }


    }
}