using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HoboKing;
using Xunit;

namespace HoboKing.Tests
{
    public class PlayerCreationTests
    {
        private readonly MapComponent map;
        private HoboKingGame game;

        public PlayerCreationTests()
        {
            game = new HoboKingGame();
            game.Run();
            map = game.multiplayerGame;
        }

        [Fact]
        public void CreateMainPlayer()
        {
            var player = map.CreateMainPlayer();
            game.Exit();
            Assert.False(player.IsOtherPlayer);
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
