using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoboKing;
using HoboKing.Components;
using HoboKing.Control;
using HoboKing.Entities;
using HoboKing.Graphics;
using HoboKing.Scenes;
using HoboKing.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using tainicom.Aether.Physics2D.Dynamics;
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
        public void TestPlayerDeepCopy()
        {
            World world = new World();
            Player p = new Player(null, new Vector2(0, 0), "", false, world);
            Player deep = p.DeepCopy() as Player;
            deep.ConnectionId = "Changed";
            Assert.NotEqual(p.ConnectionId, deep.ConnectionId);
        }

        [Fact]
        public void PlayerPressedF2Test()
        {
            World world = new World();
            Player p = new Player(null, new Vector2(0, 0), "", false, world);

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

        [Theory]
        [InlineData(0)]
        [InlineData(3)]
        [InlineData(5)]
        public void CritterUpdateTest(double time)
        {
            var critter = new CritterBuilder().AddTexture(ContentLoader.GrassLeft,
                new Vector2(0, 0)).AddMovement().Build() as Critter;
            var oldPosition = critter.Position.X;
            critter.Update(new GameTime(TimeSpan.Zero, TimeSpan.FromSeconds(time)));
            var newPosition = critter.Position.X;
            Assert.NotEqual(oldPosition, newPosition);
        }

        [Fact]
        public void MenuItemsSelectNext()
        {
            MenuItemsComponent menu = new MenuItemsComponent(_data.game, new Vector2(0, 0), Color.White, Color.White, 1);
            menu.AddMenuItem("one");
            menu.AddMenuItem("two");

            var oldSelection = menu.selectedMenuItem;
            menu.SelectPrevious();
            var newSelection = menu.selectedMenuItem;

            Assert.NotEqual(oldSelection, newSelection);
        }

        [Fact]
        public void MenuItemsSelectNextSingle()
        {
            MenuItemsComponent menu = new MenuItemsComponent(_data.game, new Vector2(0, 0), Color.White, Color.White, 1);
            menu.AddMenuItem("one");

            var oldSelection = menu.selectedMenuItem;
            menu.SelectNext();
            var newSelection = menu.selectedMenuItem;

            Assert.Equal(oldSelection, newSelection);
        }

        [Fact]
        public void MenuItemsSelectPrevious()
        {
            MenuItemsComponent menu = new MenuItemsComponent(_data.game, new Vector2(0, 0), Color.White, Color.White, 1);
            menu.AddMenuItem("one");
            menu.AddMenuItem("two");

            var oldSelection = menu.selectedMenuItem;
            menu.SelectNext();
            var newSelection = menu.selectedMenuItem;

            Assert.NotEqual(oldSelection, newSelection);
        }

        [Fact]
        public void MenuItemsSelectPreviousSingle()
        {
            MenuItemsComponent menu = new MenuItemsComponent(_data.game, new Vector2(0, 0), Color.White, Color.White, 1);
            menu.AddMenuItem("one");            

            var oldSelection = menu.selectedMenuItem;
            menu.SelectPrevious();
            var newSelection = menu.selectedMenuItem;

            Assert.Equal(oldSelection, newSelection);
        }
    }
}
