using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace SignalR_GameServer.Hubs
{
    public static class PlayerHandler
    {
        public static List<String> ConnectedIds = new List<String>();
    }

    public class GameHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            PlayerHandler.ConnectedIds.Add(Context.ConnectionId);

            _ = SendMessage("Server", $"A new player has connected with ID {Context.ConnectionId}");
            _ = PlayerConnected($"{Context.ConnectionId}");
            _ = SendPlayerCount();

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            PlayerHandler.ConnectedIds.Remove(Context.ConnectionId);

            _ = SendMessage("Server", $"Player with ID {Context.ConnectionId} has disconnected");
            _ = PlayerDisconnected($"{Context.ConnectionId}");
            _ = SendPlayerCount();

            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(string playerId, string message)
        {
            await Clients.Others.SendAsync("ReceiveMessage", playerId, message);
        }

        public async Task SendCoordinates(string playerId, float x, float y)
        {
            await Clients.Others.SendAsync("ReceiveCoordinates", playerId, x, y);
        }

        public async Task SendPlayerCount()
        {
            int player_count = PlayerHandler.ConnectedIds.Count;
            await Clients.All.SendAsync("ReceivePlayerCount", player_count);
        }

        public async Task PlayerConnected(string playerId)
        {
            await Clients.Others.SendAsync("PlayerConnected", playerId);
        }

        public async Task PlayerDisconnected(string playerId)
        {
            await Clients.Others.SendAsync("PlayerDisconnected", playerId);
        }

        // client sided?
        // junk junk KreipinysIServer.SendAsync("SendCoordinates", user, x, y)???
    }
}
