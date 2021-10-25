using HoboKing.Control;
using HoboKing.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using tainicom.Aether.Physics2D.Dynamics;

namespace HoboKing.Entities
{
    abstract class StuffBuilder
    {
        public StuffBuilder()
        {
            Reset();
        }

        protected IGameEntity entity;

        public abstract void Reset();

        public abstract StuffBuilder AddTexture(Texture2D texture, Vector2 position, int size);
        public abstract StuffBuilder AddMovement();

        public abstract IGameEntity Build();
    }
}
