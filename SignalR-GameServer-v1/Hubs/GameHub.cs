using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System;

namespace SignalR_GameServer.Hubs
{
    public class GameHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendCoordinates(string user, string x, string y)
        {
            await Clients.All.SendAsync("ReceiveCoordinates", user, x, y);
        }

        // client sided?
        // junk junk KreipinysIServer.SendAsync("SendCoordinates", user, x, y)???
    }
}
