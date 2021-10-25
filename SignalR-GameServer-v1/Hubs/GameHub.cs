using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System;

namespace SignalR_GameServer.Hubs
{
    public class GameHub : Hub
    {
        public PlayerHandler _PlayerHandler = PlayerHandler.getInstance();
             
        public override Task OnConnectedAsync()
        {
            // Attach?
            _PlayerHandler.ConnectedIds.Add(Context.ConnectionId);

            _ = SendMessage("Server", $"A new player has connected with ID {Context.ConnectionId}");
            _ = PlayerConnected($"{Context.ConnectionId}");
            _ = SendPlayerCount();

            foreach (string connectedId in _PlayerHandler.ConnectedIds)
                if(connectedId != Context.ConnectionId)
                    _ = SendPlayerIdTargeted(connectedId);

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            // Detach?
            _PlayerHandler.ConnectedIds.Remove(Context.ConnectionId);

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
            int player_count = _PlayerHandler.ConnectedIds.Count;
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

        public async Task SendPlayerIdTargeted(string playerId)
        {
            await Clients.Caller.SendAsync("PlayerConnected", playerId);
        }
    }
}
