using HoboKing.Entities;
using HoboKing.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using Xunit;

namespace HoboKing.Tests
{
    [ExcludeFromCodeCoverage]
    public class ConnectorTests
    {
        private HoboKingGame game;
        private ConnectorComponent connector;
        public ConnectorTests()
        {
            game = new HoboKingGame();

            //game.GraphicsDevice.Reset();

            //connector = new ConnectorComponent(game);
            //map = new MapComponent(game, connector);
            //map.Initialize();


            game.RunOneFrame();

            var components = game.multiplayerScene.ReturnComponents();
            foreach (var component in components)
            {
                if (component is ConnectorComponent)
                    connector = component as ConnectorComponent;
            }
        }

        [Fact]
        public void SendData()
        {
            bool worked = true;
            connector.Connect();
            Thread.Sleep(5000);
            try
            {
                connector.SendData(new GameTime(new TimeSpan(0, 0, 10), new TimeSpan(0, 0, 5)), new Vector2(20, 20));
            }
            catch (Exception)
            {
                worked = false;
            }

            connector.Disconnect();
            Thread.Sleep(5000);
            Assert.True(worked);
        }
    }
}
