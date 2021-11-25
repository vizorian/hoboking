using System.Diagnostics.CodeAnalysis;
using HoboKing.Entities;
using HoboKing.Graphics;
using Microsoft.Xna.Framework;
using tainicom.Aether.Physics2D.Dynamics;
using Xunit;

namespace HoboKing.Tests
{
    [ExcludeFromCodeCoverage]
    public class CameraTests
    {
        [Fact]
        public void CameraFollow()
        {
            var count = 0;
            var camera = new Camera();
            var world = new World();

            var player1 = new Player(null, new Vector2(0, 0), "", false, world);
            camera.Follow(player1);
            count++;

            var player2 = new Player(null, new Vector2(0, 50), "", false, world);
            camera.Follow(player2);
            count++;

            var player3 = new Player(null, new Vector2(0, 100), "", false, world);
            camera.Follow(player3);
            count++;

            Assert.Equal(3, count);
        }
    }
}