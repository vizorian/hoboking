using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace HoboKing.Control
{
    abstract class Movement
    {
        public abstract void Walk(string direction, GameTime gameTime);
        public abstract void Jump(long jumpStrength, int xDirection);
        public abstract bool BeginCharge(GameTime gameTime);
        public abstract void Idle();
    }
}
