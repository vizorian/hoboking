using System;
using System.Diagnostics;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Xna.Framework;
using System.Windows;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace HoboKing.Entities
{
    // Singleton Connector class
    class ConnectorComponent : GameComponent
    {
        private HoboKingGame hoboKingGame;
        private static HubConnection hubConnection;
        private MapComponent attachedMapComponent;


        private float timer = 0.1f;
        const float TIMER = 0.1f;
        
        public int PlayerCount { get; set; }

        public List<string> ConnectionsIds = new List<string>();
        public List<Coordinate> UnprocessedInputs = new List<Coordinate>();

        bool isConnected = false;

        // Responsible for all communication between the Client and Server
        public ConnectorComponent(HoboKingGame hoboKingGame) : base(hoboKingGame)
        {
            this.hoboKingGame = hoboKingGame;
        }

        [ExcludeFromCodeCoverage]
        public void CreateListeners()
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
            //while (hubConnection.ConnectionId == null);
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

        // Attempts to stop connection
        public async void Disconnect()
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
        public async void SendData(GameTime gameTime, Vector2 position)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            timer -= elapsed;
            if (timer < 0)
            {
                try
                {
                    await hubConnection.SendAsync("SendCoordinates", hubConnection.ConnectionId, position.X, position.Y);
                }
                catch (Exception)
                {
                    throw new Exception("Duomenų siuntimo klaida");
                }

                timer = TIMER;
            }
        }

        public override void Initialize()
        {
            CreateListeners();

            // Find the attached MapComponent
            //foreach (GameComponent component in hoboKingGame.multiplayerScene.ReturnComponents())
            //{
            //    if (component is MapComponent)
            //        attachedMapComponent = component as MapComponent;
            //}

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            // Do once depending on state
            //if (attachedMapComponent.gameState == MapComponent.GameState.Playing && !isConnected)
            //{
            //    Connect();
            //    isConnected = true;
            //}

            //SendData(gameTime);
            base.Update(gameTime);
        }
    }
}
