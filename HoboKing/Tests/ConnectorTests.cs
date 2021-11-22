using HoboKing.Entities;
using HoboKing.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Xunit;

namespace HoboKing.Tests
{
    [ExcludeFromCodeCoverage]
    public class ConnectorTests
    {
        private MapComponent map;
        private HoboKingGame game;
        private ConnectorComponent connector;
        public ConnectorTests()
        {
            game = new HoboKingGame();
            game.RunOneFrame();

            map = game.multiplayerGame;
            var components = game.multiplayerScene.ReturnComponents();
            foreach (var component in components)
            {
                if (component is ConnectorComponent)
                    connector = component as ConnectorComponent;
            }
        }

        [Fact]
        public void UpdateConnectedPlayers()
        {
            connector.ConnectionsIds.Add("testId");
            map.AddConnectedPlayers();

            var players = EntityManager.Entities.Where(p => p is Player);
            foreach (var player in players)
            {
                var actualPlayer = player as Player;
                if (actualPlayer.IsOtherPlayer && actualPlayer.ConnectionId == "testId")
                {
                    var testingPlayer = actualPlayer;

                    connector.UnprocessedInputs.Add(new Coordinate("testId", 20, 20));
                    map.UpdateConnectedPlayers();

                    var expected = new Vector2(20, 20);

                    Assert.Equal(expected, testingPlayer.Position);
                }
            }
        }

        [Fact]
        public void RemoveConnectedPlayers()
        {
            connector.ConnectionsIds.Add("testId");
            map.AddConnectedPlayers();

            var players = EntityManager.Entities.Where(p => p is Player);
            foreach (var player in players)
            {
                var actualPlayer = player as Player;
                if (actualPlayer.IsOtherPlayer && actualPlayer.ConnectionId == "testId")
                {
                    connector.ConnectionsIds.Remove("testId");
                    map.RemoveConnectedPlayers();

                    Assert.Null(actualPlayer);
                }
            }
        }
    }
}
