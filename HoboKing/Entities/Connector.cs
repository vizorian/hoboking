using System;
using System.Diagnostics;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Xna.Framework;
using System.Windows;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace HoboKing.Entities
{
    // Singleton Connector class
    class Connector
    {
        private static readonly object lockObj = new object();
        private static Connector connector; 
        private static HubConnection hubConnection;
        
        public int PlayerCount { get; set; }

        public List<string> ConnectionsIds = new List<string>();
        public List<Coordinate> UnprocessedInputs = new List<Coordinate>();

        // Responsible for all communication between the Client and Server
        private Connector()
        {
            // Create a connection to the Server's hub with automatic reconnection
            hubConnection = new HubConnectionBuilder().
                WithUrl("https://hoboking-appservice.azurewebsites.net/gameHub").WithAutomaticReconnect().Build();
            
            // Receives a message from the Server
            // Kept for testing or possible chat
            hubConnection.On<string, string>("ReceiveMessage", (user, msg) => Console.WriteLine($"{user}: {msg}"));

            // Adds a new connection to the current connection list
            hubConnection.On<string>("PlayerConnected", (connectionId) => ConnectionsIds.Add(connectionId));

            // Removes a connection from the current connection list
            hubConnection.On<string>("PlayerDisconnected", (connectionId) => ConnectionsIds.Remove(connectionId));

            // Adds incoming inputs to the input list
            hubConnection.On<string, float, float>("ReceiveCoordinates", (id, x, y) => UnprocessedInputs.Add(new Coordinate(id, x, y)));

            // Updates current player count
            // Might use for checking if the current connections match the player count in the Server
            hubConnection.On<int>("ReceivePlayerCount", (player_count) => PlayerCount = player_count);
        }

        // Get self connection ID, but wait for a connection
        public string GetConnectionId()
        {
            // fuck it, can't find a better way
            while (hubConnection.ConnectionId == null);
            return hubConnection.ConnectionId;
        }

        // Attempts to initiate connection
        public async void Connect()
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

        // Sends own positional coordinates to Server
        public async void SendCoordinates(Vector2 position)
        {
            try
            {
                await hubConnection.SendAsync("SendCoordinates", 
                    hubConnection.ConnectionId, position.X, position.Y);
            }
            catch (Exception)
            {
                throw new Exception("Duomenų siuntimo klaida");
            }
        }

        // Gets the Connector instance or makes a new one if it's not created
        public static Connector getInstance()
        {
            if (connector == null)
            {
                lock (lockObj)
                {
                    if (connector == null)
                    {
                        connector = new Connector();
                    }
                }
            }
            return connector;
        }
    }
}
