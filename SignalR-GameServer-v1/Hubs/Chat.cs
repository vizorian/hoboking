using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR_GameServer.Hubs
{
    public class Chat : Hub
    {
        public async Task BroadcastMessage(string name, string message)
        {
            await Clients.All.SendAsync("broadcastMessage", name, message);
        }

        public async Task Echo(string name, string message)
        {
            await Clients.Client(Context.ConnectionId).SendAsync("echo", name, message + " (echo from server)");
        }
    }
}
