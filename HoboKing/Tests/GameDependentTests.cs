using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using HoboKing.Builder;
using HoboKing.Components;
using HoboKing.Composite;
using HoboKing.Control;
using HoboKing.Entities;
using HoboKing.Graphics;
using HoboKing.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using tainicom.Aether.Physics2D.Dynamics;
using Xunit;

namespace HoboKing.Tests
{
    public class TestsFixture : IDisposable
    {
        internal Connector Connector;
        internal HoboKingGame Game;
        internal MapComponent Map;

        public TestsFixture()
        {
            // Do "global" initialization here; Only called once.
            Game = new HoboKingGame();
            Game.RunOneFrame();

            //connector = new Connector();
            //connector.CreateListeners();

            var components = Game.Components;
            foreach (var component in components)
                if (component is MapComponent mapComponent)
                    Map = mapComponent;

            if (Map != null) Connector = Map.Connector;
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
        private readonly TestsFixture data;

        public GameDependentTests(TestsFixture data)
        {
            this.data = data;
        }

        [Fact]
        public void CreateMainPlayer()
        {
            var player = data.Map.CreateMainPlayer();
            Assert.False(player.IsOtherPlayer);
        }

        [Fact]
        public void TestPlayerDeepCopy()
        {
            var world = new World();
            var p = new Player(null, new Vector2(0, 0), "", false, world);
            if (p.DeepCopy() is Player deep)
            {
                deep.ConnectionId = "Changed";
                Assert.NotEqual(p.ConnectionId, deep.ConnectionId);
            }
        }

        [Fact]
        public void PlayerPressedF2Test()
        {
            var world = new World();
            var p = new Player(null, new Vector2(0, 0), "", false, world);

            InputController.PreviousKeyboardState = new KeyboardState(Keys.None);
            InputController.KeyboardState = new KeyboardState(Keys.F2);

            p.Update(new GameTime());
            p.Update(new GameTime());
            p.Update(new GameTime());

            Assert.True(true);
            InputController.PreviousKeyboardState = new KeyboardState();
            InputController.KeyboardState = new KeyboardState();
        }

        [Fact]
        public void CreateOtherPlayer()
        {
            data.Connector.ConnectionsIds.Add("testId");
            data.Map.AddConnectedPlayers();

            var players = EntityManager.EntitiesNum.Where(p => p is Player);
            foreach (var player in players)
            {
                if (player is Player {IsOtherPlayer: true, ConnectionId: "testId"} actualPlayer)
                    Assert.Equal("testId", actualPlayer.ConnectionId);
            }
        }

        [Fact]
        public void UpdateConnectedPlayers()
        {
            data.Connector.ConnectionsIds.Add("testId");
            data.Map.AddConnectedPlayers();
            data.Map.Update(new GameTime());

            data.Connector.UnprocessedInputs.Add(new Coordinate("testId", 20, 20));
            data.Connector.UnprocessedInputs.Add(new Coordinate("nonExistant", 20, 20));
            data.Map.UpdateConnectedPlayers();

            var expected = new Vector2(20, 20);

            var players = EntityManager.EntitiesNum.Where(p => p is Player);

            if (players.FirstOrDefault(p => (p as Player)?.ConnectionId == "testId") is Player player) Assert.Equal(expected, player.Position);
        }

        [Fact]
        public void RemoveConnectedPlayers()
        {
            data.Connector.ConnectionsIds.Add("testId");
            data.Map.AddConnectedPlayers();
            data.Map.Update(new GameTime());

            data.Connector.ConnectionsIds.Remove("testId");
            data.Map.RemoveConnectedPlayers();

            var players = EntityManager.EntitiesNum.Where(p => p is Player);

            if (players.FirstOrDefault(p => (p as Player)?.ConnectionId == "testId") is Player player) Assert.Equal("testId", player.ConnectionId);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(3)]
        [InlineData(5)]
        public void CritterUpdateTest(double time)
        {
            if (new CritterBuilder().AddTexture(ContentLoader.GrassLeft,
                new Vector2(0, 0)).AddMovement().Build() is Critter critter)
            {
                var oldPosition = critter.Position.X;
                critter.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(time)));
                var newPosition = critter.Position.X;
                Assert.NotEqual(oldPosition, newPosition);
            }
        }

        //[Fact]
        //public void MenuItemsSelectNext()
        //{
        //    var menu = new MenuItemsComponent(data.Game, new Vector2(0, 0), Color.White, Color.White, 1);
        //    menu.AddItem("one");
        //    menu.AddItem("two");

        //    var oldSelection = menu.SelectedMenuItem;
        //    menu.SelectPrevious();
        //    var newSelection = menu.SelectedMenuItem;

        //    Assert.NotEqual(oldSelection, newSelection);
        //}

        //[Fact]
        //public void MenuItemsSelectNextSingle()
        //{
        //    var menu = new MenuItemsComponent(data.Game, new Vector2(0, 0), Color.White, Color.White, 1);
        //    menu.AddItem("one");

        //    var oldSelection = menu.SelectedMenuItem;
        //    menu.SelectNext();
        //    var newSelection = menu.SelectedMenuItem;

        //    Assert.Equal(oldSelection, newSelection);
        //}

        //[Fact]
        //public void MenuItemsSelectPrevious()
        //{
        //    var menu = new MenuItemsComponent(data.Game, new Vector2(0, 0), Color.White, Color.White, 1);
        //    menu.AddItem("one");
        //    menu.AddItem("two");

        //    var oldSelection = menu.SelectedMenuItem;
        //    menu.SelectNext();
        //    var newSelection = menu.SelectedMenuItem;

        //    Assert.NotEqual(oldSelection, newSelection);
        //}

        //[Fact]
        //public void MenuItemsSelectPreviousSingle()
        //{
        //    var menu = new MenuItemsComponent(data.Game, new Vector2(0, 0), Color.White, Color.White, 1);
        //    menu.AddItem("one");

        //    var oldSelection = menu.SelectedMenuItem;
        //    menu.SelectPrevious();
        //    var newSelection = menu.SelectedMenuItem;

        //    Assert.Equal(oldSelection, newSelection);
        //}
    }
}