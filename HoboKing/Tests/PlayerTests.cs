using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoboKing;
using HoboKing.Entities;
using HoboKing.Graphics;
using HoboKing.Scenes;
using Microsoft.Xna.Framework;
using Xunit;

namespace HoboKing.Tests
{
    [ExcludeFromCodeCoverage]
    public class PlayerTests
    {
        private MapComponent map;
        private HoboKingGame game;
        private ConnectorComponent connector;

        public PlayerTests()
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
        public void CreateMainPlayer()
        {
            var player = map.CreateMainPlayer();
            Assert.False(player.IsOtherPlayer);
        }

        [Fact]
        public void CreateOtherPlayer()
        {
            connector.ConnectionsIds.Add("testId");
            map.AddConnectedPlayers();

            var players = EntityManager.Entities.Where(p => p is Player);
            foreach (var player in players)
            {
                var actualPlayer = player as Player;
                if (actualPlayer.IsOtherPlayer && actualPlayer.ConnectionId == "testId")
                {
                    Assert.Equal("testId", actualPlayer.ConnectionId);
                }
            }
        }

        //[Theory]
        //[InlineData(6, 9)]
        //[InlineData(3, 2)]
        //public void CreateOtherPlayer(int x, int y)
        //{
        //    map.Connector
        //    var player = map.CreateMainPlayer();
        //    Assert.False(player.IsOtherPlayer);
        //}
    }
}
