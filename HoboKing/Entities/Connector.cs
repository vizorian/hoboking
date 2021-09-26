using System;
using System.Diagnostics;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Xna.Framework;
using System.Windows;
using System.Threading.Tasks;

namespace HoboKing.Entities
{
    class Connector
    {
        private HubConnection Connection;

        public Connector()
        {
            Connection = new HubConnectionBuilder().
                WithUrl("https://hoboking-appservice.azurewebsites.net/gameHub").WithAutomaticReconnect().Build();
            Connection.On<string, string>("ReceiveMessage", (user, x) =>
            {
                Console.WriteLine($"User: {user}");
                Console.WriteLine($"X: {x}");
                //Console.WriteLine($"Y: {y}");
            });
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

        public async void SendMsg(Vector2 position)
        {
            try
            {
                await Connection.SendAsync("SendMessage", "Pikynas", "kil niG");
            }
            catch (Exception)
            {
                throw new Exception("Duomenų siuntimo klaida");
            }
        }

        public async void Send(Vector2 position)
        {
            try
            {
                await Connection.SendAsync("SendCoordinates", "Pikynas", position.X.ToString(), position.Y.ToString());
            }
            catch (Exception)
            {
                throw new Exception("Duomenų siuntimo klaida");
            }
        }

        public async void Receive()
        {

        }
    }
}
