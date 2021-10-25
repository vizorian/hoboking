using HoboKing.Control;
using HoboKing.Control.Strategy;
using HoboKing.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using tainicom.Aether.Physics2D.Dynamics;

namespace HoboKing.Entities
{
    class ObjectBuilder : StuffBuilder
    {
        public override StuffBuilder AddMovement()
        {
            (entity as Object).SetMovementStrategy(new CritterMovement(entity));
            return this;
        }

        public override StuffBuilder AddTexture(Texture2D texture, Vector2 position, int size)
        {
            // Recalculates tiles to absolute coordinates
            position.X *= MapComponent.TILE_SIZE;
            position.Y *= MapComponent.TILE_SIZE;

            entity.Sprite = new Sprite(texture, position, size);
            return this;
        }

        public StuffBuilder AddSpeech(string speechText, int triggerSize)
        {
            return this;
        }

        public override IGameEntity Build()
        {
            Object finalEntity = entity as Object;
            Reset();
            return finalEntity;
        }

        public override void Reset()
        {
            entity = new Object();
        }
    }
}
