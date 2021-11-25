using HoboKing.Components;
using HoboKing.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoboKing.Builder
{
    internal abstract class AbstractBuilder
    {
        protected GameEntity entity;

        public abstract void Reset();

        public AbstractBuilder AddTexture(Texture2D texture, Vector2 positionInGrid, int size = 0)
        {
            // Recalculates tiles to absolute coordinates
            positionInGrid.X *= MapComponent.TILE_SIZE;
            positionInGrid.Y *= MapComponent.TILE_SIZE;

            entity.ChangeTexture(texture);
            entity.ChangePosition(positionInGrid);
            entity.ChangeTileSize(size);

            return this;
        }

        public abstract AbstractBuilder AddMovement();

        public abstract GameEntity Build();
    }
}