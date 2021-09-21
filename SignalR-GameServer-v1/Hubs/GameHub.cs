using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System;

namespace SignalR_GameServer.Hubs
{
    public class GameHub : Hub
    {
        public async Task MovePlayer(string x, string y)
        {
            await Clients.Others.SendAsync("playerMoved", x, y);
        }
    }
}
