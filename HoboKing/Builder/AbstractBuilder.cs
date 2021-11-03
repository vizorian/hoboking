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
    abstract class AbstractBuilder
    {
        public AbstractBuilder()
        {
            Reset();
        }

        protected GameEntity entity;

        public abstract void Reset();

        public AbstractBuilder AddTexture(Texture2D texture, Vector2 positionInGrid, int size = 0)
        {
            // Recalculates tiles to absolute coordinates
            positionInGrid.X *= MapComponent.TILE_SIZE;
            positionInGrid.Y *= MapComponent.TILE_SIZE;

            entity.ChangeTexture(texture);
            entity.ChangePosition(positionInGrid);
            entity.ChangeSize(size);

            return this;
        }
        public abstract AbstractBuilder AddMovement();

        public abstract GameEntity Build();
    }
}
