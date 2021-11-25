using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using HoboKing.Utils;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Xna.Framework;

namespace HoboKing.Components
{
    // Singleton Connector class
    internal class ConnectorComponent
    {
        private const float TIMER = 0.1f;
        private static HubConnection hubConnection;

        public List<string> ConnectionsIds = new List<string>();

        private float timer = 0.1f;
        public List<Coordinate> UnprocessedInputs = new List<Coordinate>();

        // Responsible for all communication between the Client and Server

        public int PlayerCount { get; set; }

        [ExcludeFromCodeCoverage]
        public void CreateListeners()
        {
            // Create a connection to the Server's hub with automatic reconnection
            hubConnection = new HubConnectionBuilder().WithUrl("https://hoboking-appservice.azurewebsites.net/gameHub")
                .WithAutomaticReconnect().Build();

            // Receives a message from the Server
            // Kept for testing or possible chat
            hubConnection.On<string, string>("ReceiveMessage", (user, msg) => Console.WriteLine($"{user}: {msg}"));

            // Adds a new connection to the current connection list
            hubConnection.On<string>("PlayerConnected", connectionId => ConnectionsIds.Add(connectionId));

            // Removes a connection from the current connection list
            hubConnection.On<string>("PlayerDisconnected", connectionId => ConnectionsIds.Remove(connectionId));

            // Adds incoming inputs to the input list
            hubConnection.On<string, float, float>("ReceiveCoordinates",
                (id, x, y) => UnprocessedInputs.Add(new Coordinate(id, x, y)));

            // Updates current player count
            // Might use for checking if the current connections match the player count in the Server
            hubConnection.On<int>("ReceivePlayerCount", playerCount => PlayerCount = playerCount);
        }

        // Get self connection ID, but wait for a connection
        public string GetConnectionId()
        {
            // fuck it, can't find a better way
            //while (hubConnection.ConnectionId == null);
            return hubConnection.ConnectionId;
        }

        // Attempts to initiate connection
        public async Task Connect()
        {
            try
            {
                await hubConnection.StartAsync();
            }
            catch (Exception)
            {
                throw new Exception("Prisijungimo klaida!");
            }

            Console.WriteLine("Connection state: " + hubConnection.State);
        }

        // Attempts to stop connection
        public async Task Disconnect()
        {
            try
            {
                await hubConnection.StopAsync();
            }
            catch (Exception)
            {
                throw new Exception("Atsijungimo klaida!");
            }

            Console.WriteLine("Connection state: " + hubConnection.State);
        }

        // Send own positional coordinates every time interval
        public async Task SendData(GameTime gameTime, Vector2 position)
        {
            var elapsed = (float) gameTime.ElapsedGameTime.TotalSeconds;
            timer -= elapsed;
            if (timer < 0)
            {
                try
                {
                    await hubConnection.SendAsync("SendCoordinates", hubConnection.ConnectionId, position.X,
                        position.Y);
                }
                catch (Exception)
                {
                    throw new Exception("Duomenų siuntimo klaida");
                }

                timer = TIMER;
            }
        }
    }
}