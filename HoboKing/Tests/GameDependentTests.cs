﻿using System;
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
using Microsoft.Xna.Framework.Graphics;
using Xunit;

namespace HoboKing.Tests
{
    public class TestsFixture : IDisposable
    {
        internal MapComponent map;
        internal HoboKingGame game;
        internal ConnectorComponent connector;

        public TestsFixture()
        {
            // Do "global" initialization here; Only called once.
            game = new HoboKingGame();
            game.RunOneFrame();

            connector = new ConnectorComponent();
            connector.CreateListeners();

            var components = game.multiplayerScene.ReturnComponents();
            foreach (var component in components)
            {
                //if (component is ConnectorComponent)
                //    connector = component as ConnectorComponent;
                if (component is MapComponent)
                    map = component as MapComponent;
            }
        }

        public void Dispose()
        {
            // Do "global" teardown here; Only called once.
            //game.Exit();
        }
    }

    [ExcludeFromCodeCoverage]
    public class GameDependentTests : IClassFixture<TestsFixture>
    {
        private TestsFixture _data;

        public GameDependentTests(TestsFixture data)
        {
            _data = data;
        }

        [Fact]
        public void CreateMainPlayer()
        {

            var player = _data.map.CreateMainPlayer();
            Assert.False(player.IsOtherPlayer);
        }

        [Fact]
        public void CreateOtherPlayer()
        {
            _data.connector.ConnectionsIds.Add("testId2");
            _data.map.AddConnectedPlayers();

            var players = EntityManager.Entities.Where(p => p is Player);
            foreach (var player in players)
            {
                var actualPlayer = player as Player;
                if (actualPlayer.IsOtherPlayer && actualPlayer.ConnectionId == "testId2")
                {
                    Assert.Equal("testId2", actualPlayer.ConnectionId);
                }
            }
        }

        [Fact]
        public void UpdateConnectedPlayers()
        {
            _data.connector.ConnectionsIds.Add("testId");
            _data.map.AddConnectedPlayers();

            var players = EntityManager.Entities.Where(p => p is Player);
            foreach (var player in players)
            {
                var actualPlayer = player as Player;
                if (actualPlayer.IsOtherPlayer && actualPlayer.ConnectionId == "testId")
                {
                    var testingPlayer = actualPlayer;

                    _data.connector.UnprocessedInputs.Add(new Coordinate("testId", 20, 20));
                    _data.map.UpdateConnectedPlayers();

                    var expected = new Vector2(20, 20);

                    Assert.Equal(expected, testingPlayer.Position);
                }
            }
        }

        [Fact]
        public void RemoveConnectedPlayers()
        {
            _data.connector.ConnectionsIds.Add("testId");
            _data.map.AddConnectedPlayers();

            var players = EntityManager.Entities.Where(p => p is Player);
            foreach (var player in players)
            {
                var actualPlayer = player as Player;
                if (actualPlayer.IsOtherPlayer && actualPlayer.ConnectionId == "testId")
                {
                    _data.connector.ConnectionsIds.Remove("testId");
                    _data.map.RemoveConnectedPlayers();
                    Assert.Null(actualPlayer);
                }
            }
        }

        [Fact]
        public void CritterUpdateTest()
        {
            var critter = new CritterBuilder().AddTexture(ContentLoader.GrassLeft,
                new Vector2(0, 0)).AddMovement().Build() as Critter;
            var oldPosition = critter.Position.X;
            critter.Update(new GameTime());
            var newPosition = critter.Position.X;
            Assert.NotEqual(oldPosition, newPosition);
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