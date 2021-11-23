using HoboKing.Entities;
using HoboKing.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
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
            int count = 0;
            Camera camera = new Camera();
            World world = new World();

            Player player1 = new Player(null, new Vector2(0, 0), "", false, world);
            camera.Follow(player1);
            count++;

            Player player2 = new Player(null, new Vector2(0, 50), "", false, world);
            camera.Follow(player2);
            count++;

            Player player3 = new Player(null, new Vector2(0, 100), "", false, world);
            camera.Follow(player3);
            count++;

            Assert.Equal(expected: 3, count);
        }

    }
}
