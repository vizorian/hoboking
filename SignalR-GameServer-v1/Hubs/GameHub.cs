using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System;

namespace SignalR_GameServer_v1.Hubs
{
    public class GameHub : Hub
    {
        public GameHub()
        {
        }

        public async Task SendMessage(string user, string message)
        {
            //await Clients.All.SendAsync("ReceiveMessage", user, message);
            await Clients.Others.SendAsync("ReceiveMessage", user, message);
            //await Clients.Caller.SendAsync("ReceiveMessage", user, "delivered: " + message);
        }
    }
}
