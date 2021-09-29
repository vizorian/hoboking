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

        public List<string> IDs = new List<string>();
        public List<Coordinate> coords = new List<Coordinate>();

        public Connector()
        {
            Connection = new HubConnectionBuilder().
                WithUrl("https://hoboking-appservice.azurewebsites.net/gameHub").WithAutomaticReconnect().Build();
            
            // Receives a message from the server
            Connection.On<string, string>("ReceiveMessage", (user, msg) =>
            {
                Console.WriteLine($"USER: {user}");
                Console.WriteLine($"MESSAGE: {msg}");
            });

            // Updates player count
            Connection.On<string>("PlayerConnected", (connectionId) =>
            {
                IDs.Add(connectionId);
            });

            Connection.On<string>("PlayerDisconnected", (connectionId) =>
            {
                IDs.Remove(connectionId);
            });

            Connection.On<string, float, float>("ReceiveCoordinates", (id, x, y) =>
            {
                Console.WriteLine("Received data from" + id + "X: " + x + "Y: " + y);
                coords.Add(new Coordinate(id, x, y));
            });

            Connection.On<int>("ReceivePlayerCount", (player_count) =>
            {
                Console.WriteLine("Received count data from server. Current player count: " + player_count);
            });
        }

        public string GetConnectionID()
        {
            return Connection.ConnectionId;
        }

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
            Console.WriteLine("Connection STATE: " + Connection.State);
        }

        public async void Send(Vector2 position)
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
