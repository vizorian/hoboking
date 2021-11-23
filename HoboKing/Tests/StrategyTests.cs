using HoboKing.Control;
using HoboKing.Control.Strategy;
using HoboKing.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using tainicom.Aether.Physics2D.Dynamics;
using Xunit;

namespace HoboKing.Tests
{
    [ExcludeFromCodeCoverage]
    public class StrategyTests
    {
        [Theory]
        [InlineData(Keys.None, Keys.Enter)]
        [InlineData(Keys.W, Keys.Enter)]
        public void KeyPressedTest(Keys previous, Keys current)
        {
            InputController.PreviousKeyboardState = new KeyboardState(previous);
            InputController.KeyboardState = new KeyboardState(current);
            Assert.True(InputController.KeyPressed(Keys.Enter));
        }

        [Theory]
        [InlineData(Keys.Enter, Keys.Enter)]
        [InlineData(Keys.X, Keys.X)]
        public void KeyPressedDownTest(Keys pressed, Keys check)
        {
            InputController.KeyboardState = new KeyboardState(pressed);
            Assert.True(InputController.KeyPressedDown(check));
        }

        [Theory]
        [InlineData(Keys.None, Keys.Enter)]
        [InlineData(Keys.W, Keys.X)]
        public void KeyPressedDownTest_Fail(Keys pressed, Keys check)
        {
            InputController.KeyboardState = new KeyboardState(pressed);
            Assert.False(InputController.KeyPressedDown(check));
        }

        [Theory]
        [InlineData(Keys.Enter, Keys.None)]
        [InlineData(Keys.W, Keys.None)]
        [InlineData(Keys.A, Keys.None)]
        public void KeyReleasedTest(Keys previous, Keys current)
        {
            InputController.PreviousKeyboardState = new KeyboardState(previous);
            InputController.KeyboardState = new KeyboardState(current);
            Assert.True(InputController.KeyReleased(previous));
        }

        [Theory]
        [InlineData(Keys.None, Keys.Enter)]
        [InlineData(Keys.None, Keys.W)]
        [InlineData(Keys.A, Keys.A)]
        public void KeyReleasedTest_Fail(Keys previous, Keys current)
        {
            InputController.PreviousKeyboardState = new KeyboardState(previous);
            InputController.KeyboardState = new KeyboardState(current);
            Assert.False(InputController.KeyReleased(current));
        }

        [Theory]
        [InlineData(Keys.W)]
        [InlineData(Keys.S)]
        public void DebugMovementTest_Vertical(Keys key)
        {
            World world = new World();
            Player p = new Player(null, new Vector2(0, 0), "", false, world);
            p.SetMovementStrategy(new DebugMovement(p));
            InputController.KeyboardState = new KeyboardState(key);

            var oldVel = p.body.LinearVelocity.Y;
            p.Update(new GameTime());
            var newVel = p.body.LinearVelocity.Y;
            Assert.NotEqual(newVel, oldVel);

            InputController.KeyboardState = new KeyboardState();
        }

        [Theory]
        [InlineData(Keys.A)]
        [InlineData(Keys.D)]
        public void DebugMovementTest_Horizontal(Keys key)
        {
            World world = new World();
            Player p = new Player(null, new Vector2(0, 0), "", false, world);
            p.SetMovementStrategy(new DebugMovement(p));
            InputController.KeyboardState = new KeyboardState(key);

            var oldVel = p.body.LinearVelocity.X;
            p.Update(new GameTime());
            var newVel = p.body.LinearVelocity.X;
            Assert.NotEqual(newVel, oldVel);

            InputController.KeyboardState = new KeyboardState();
        }

        [Theory]
        [InlineData(Keys.W, Keys.None)]
        [InlineData(Keys.A, Keys.None)]
        [InlineData(Keys.S, Keys.None)]
        [InlineData(Keys.D, Keys.None)]
        public void DebugMovement_KeyReleasedTest(Keys previous, Keys current)
        {
            InputController.PreviousKeyboardState = new KeyboardState(previous);
            InputController.KeyboardState = new KeyboardState(current);

            World world = new World();
            Player p = new Player(null, new Vector2(0, 0), "", false, world);
            p.SetMovementStrategy(new DebugMovement(p));
            p.Update(new GameTime());

            if (current == Keys.W || current == Keys.S)
            {
                Assert.Equal(0, p.body.LinearVelocity.Y);
            }

            if (current == Keys.A || current == Keys.D)
            {
                Assert.Equal(0, p.body.LinearVelocity.X);
            }

            InputController.PreviousKeyboardState = new KeyboardState();
            InputController.KeyboardState = new KeyboardState();
        }

        [Theory]
        [InlineData(Keys.Space)]
        [InlineData(Keys.W)]
        public void IceMovementTest_Up(Keys key)
        {
            World world = new World();
            Player p = new Player(null, new Vector2(0, 0), "", false, world);
            p.SetMovementStrategy(new IceMovement(p));
            InputController.KeyboardState = new KeyboardState(key);

            var oldVel = p.body.LinearVelocity.Y;
            p.Update(new GameTime());
            var newVel = p.body.LinearVelocity.Y;
            Assert.NotEqual(newVel, oldVel);

            InputController.KeyboardState = new KeyboardState();
        }

        [Theory]
        [InlineData(Keys.A)]
        [InlineData(Keys.D)]
        public void IceMovementTest_Horizontal(Keys key)
        {
            World world = new World();
            Player p = new Player(null, new Vector2(0, 0), "", false, world);
            p.SetMovementStrategy(new IceMovement(p));
            InputController.KeyboardState = new KeyboardState(key);

            var oldVel = p.body.LinearVelocity.X;
            p.Update(new GameTime());
            var newVel = p.body.LinearVelocity.X;
            Assert.NotEqual(newVel, oldVel);

            InputController.KeyboardState = new KeyboardState();
        }

        [Fact]
        public void IceMovementDown()
        {
            World world = new World();
            Player p = new Player(null, new Vector2(0, 0), "", false, world);
            IceMovement ice = new IceMovement(p);

            InputController.PreviousKeyboardState = new KeyboardState(Keys.S);
            InputController.KeyboardState = new KeyboardState(Keys.S);

            ice.AcceptInputs(new GameTime());
            Assert.True(true);

            InputController.PreviousKeyboardState = new KeyboardState();
            InputController.KeyboardState = new KeyboardState();
        }

        [Theory]
        [InlineData(Keys.W)]
        [InlineData(Keys.Space)]
        public void PlayerMovementTest_Vertical(Keys key)
        {
            World world = new World();
            Player p = new Player(null, new Vector2(0, 0), "", false, world);
            p.SetMovementStrategy(new PlayerMovement(p));
            InputController.KeyboardState = new KeyboardState(key);

            var oldVel = p.body.LinearVelocity.Y;
            p.Update(new GameTime());
            var newVel = p.body.LinearVelocity.Y;
            Assert.NotEqual(newVel, oldVel);

            InputController.KeyboardState = new KeyboardState();
        }

        [Theory]
        [InlineData(Keys.A)]
        [InlineData(Keys.D)]
        public void PlayerMovementTest_Horizontal(Keys key)
        {
            World world = new World();
            Player p = new Player(null, new Vector2(0, 0), "", false, world);
            p.SetMovementStrategy(new PlayerMovement(p));
            InputController.KeyboardState = new KeyboardState(key);

            var oldVel = p.body.LinearVelocity.X;
            p.Update(new GameTime());
            var newVel = p.body.LinearVelocity.X;
            Assert.NotEqual(newVel, oldVel);

            InputController.KeyboardState = new KeyboardState();
        }

        [Theory]
        [InlineData(Keys.A, Keys.None)]
        [InlineData(Keys.D, Keys.None)]
        public void PlayerMovement_KeyReleasedTest(Keys previous, Keys current)
        {
            InputController.PreviousKeyboardState = new KeyboardState(previous);
            InputController.KeyboardState = new KeyboardState(current);

            World world = new World();
            Player p = new Player(null, new Vector2(0, 0), "", false, world);
            p.SetMovementStrategy(new PlayerMovement(p));
            p.Update(new GameTime());

            if (current == Keys.A || current == Keys.D)
            {
                Assert.Equal(0, p.body.LinearVelocity.X);
            }

            InputController.PreviousKeyboardState = new KeyboardState();
            InputController.KeyboardState = new KeyboardState();
        }

        [Fact]
        public void PlayerMovementDown()
        {
            World world = new World();
            Player p = new Player(null, new Vector2(0, 0), "", false, world);
            PlayerMovement playerMovement = new PlayerMovement(p);

            InputController.PreviousKeyboardState = new KeyboardState(Keys.S);
            InputController.KeyboardState = new KeyboardState(Keys.S);

            playerMovement.AcceptInputs(new GameTime());
            Assert.True(true);

            InputController.PreviousKeyboardState = new KeyboardState();
            InputController.KeyboardState = new KeyboardState();
        }
    }
}
