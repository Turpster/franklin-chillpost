using Discord.WebSocket;

namespace Discord_Bot.CommandNS.CommandsNS
{
    public class Ping : Command
    {
        public Ping(Executor executor) : base(new [] {"ping", "p"}, executor) {}

        public override void OnCommand(SocketMessage rawSocketMessage, string[] args)
        {
            rawSocketMessage.Channel.SendMessageAsync($"<@{rawSocketMessage.Author.Id}> Pong!");
        }
    }
}