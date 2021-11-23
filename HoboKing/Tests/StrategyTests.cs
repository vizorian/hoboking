using HoboKing.Control;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
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
        [InlineData(Keys.Enter, Keys.None)]
        [InlineData(Keys.W, Keys.None)]
        [InlineData(Keys.A, Keys.None)]
        public void KeyReleasedTest(Keys previous, Keys current)
        {
            InputController.PreviousKeyboardState = new KeyboardState(previous);
            InputController.KeyboardState = new KeyboardState(current);
            Assert.True(InputController.KeyReleased(previous));
        }

        [Fact]
        public void PlayerMovementTest()
        {

        }
    }
}
