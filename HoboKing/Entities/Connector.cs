using System;
using System.Diagnostics;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Xna.Framework;
using System.Windows;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace HoboKing.Entities
{
    class Connector
    {
        private HubConnection Connection;
        public int PlayerCount { get; set; }

        public List<string> Connections = new List<string>();
        public List<Coordinate> Inputs = new List<Coordinate>();

        // Responsible for all communication between the Client and Server
        public Connector()
        {
            // Create a connection to the Server's hub with automatic reconnection
            Connection = new HubConnectionBuilder().
                WithUrl("https://hoboking-appservice.azurewebsites.net/gameHub").WithAutomaticReconnect().Build();
            
            // Receives a message from the Server
            // Kept for testing or possible chat
            Connection.On<string, string>("ReceiveMessage", (user, msg) =>
            {
                Console.WriteLine($"USER: {user}");
                Console.WriteLine($"MESSAGE: {msg}");
            });

            // Adds a new connection to the current connection list
            Connection.On<string>("PlayerConnected", (connectionId) => Connections.Add(connectionId));

            // Removes a connection from the current connection list
            Connection.On<string>("PlayerDisconnected", (connectionId) => Connections.Remove(connectionId));

            // Adds incoming inputs to the input list
            Connection.On<string, float, float>("ReceiveCoordinates", (id, x, y) => Inputs.Add(new Coordinate(id, x, y)));

            // Updates current player count
            // Might use for checking if the current connections match the player count in the Server
            Connection.On<int>("ReceivePlayerCount", (player_count) => PlayerCount = player_count);
        }

        // Get self connection ID, but wait for a connection
        public string GetConnectionID()
        {
            // fuck it, can't find a better way
            while (Connection.ConnectionId == null);
            return Connection.ConnectionId;
        }

        // Attempts to initiate connection
        public async void Connect()
        {
            try
            {
                await Connection.StartAsync();
            }
            catch (Exception)
            {
                throw new Exception("Prisijungimo klaida!");
            }
            Console.WriteLine("Connection state: " + Connection.State);
        }

        // Sends own positional coordinates to Server
        public async void SendCoordinates(Vector2 position)
        {
            try
            {
                await Connection.SendAsync("SendCoordinates", 
                    Connection.ConnectionId, position.X, position.Y);
            }
            catch (Exception)
            {
                throw new Exception("Duomenų siuntimo klaida");
            }
        }
    }
}
