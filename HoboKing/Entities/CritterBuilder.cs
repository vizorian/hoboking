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
    class CritterBuilder
    {
        private Critter critter;
        public CritterBuilder()
        {
            Reset();
        }

        public void Reset()
        {
            critter = new Critter();
        }

        public CritterBuilder AddTexture(Texture2D texture, Vector2 position, int size)
        {
            // Recalculates tiles to absolute coordinates
            position.X *= MapComponent.TILE_SIZE;
            position.Y *= MapComponent.TILE_SIZE;

            critter.Sprite = new Sprite(texture, position, size);
            return this;
        }

        public CritterBuilder AddMovement()
        {
            critter.SetMovementStrategy(new CritterMovement(critter));
            return this;
        }

        public CritterBuilder AddSpeech(string speechText, int triggerSize)
        {
            return this;
        }

        public Critter Build()
        {
            Critter finalCritter = critter;
            Reset();
            return finalCritter;
        }
    }
}
