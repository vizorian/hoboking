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
    class CritterBuilder
    {
        public CritterBuilder()
        {
            Reset();
        }

        private Critter critter;

        public void Reset()
        {
            this.critter = new Critter();
        }

        public CritterBuilder AddTexture(Texture2D texture, Vector2 position, int size, World world)
        {
            // Recalculates tiles to absolute coordinates
            position.X *= MapComponent.TILE_SIZE;
            position.Y *= MapComponent.TILE_SIZE;

            critter.Sprite = new Sprite(texture, position, world, size);
            return this;
        }

        public CritterBuilder AddMovement(Movement movement)
        {
            critter.SetMovementStrategy(movement);
            return this;
        }

        public CritterBuilder AddSpeech(string speechText, int triggerSize)
        {
            return this;
        }

        public Critter Build()
        {
            Critter finalCritter = this.critter;
            this.Reset();
            return finalCritter;
        }
    }
}
