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
            Debug.WriteLine("Connection STATE: " + Connection.State);
        }

        public async void Send(Vector2 position)
        {
            try
            {
                await Connection.SendAsync("SendCoordinates", Connection.ConnectionId, position.X.ToString(), position.Y.ToString());
            }
            catch (Exception)
            {
                throw new Exception("Duomenų siuntimo klaida");
            }
        }
    }
}
