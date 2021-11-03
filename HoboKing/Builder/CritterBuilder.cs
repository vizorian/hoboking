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
    class CritterBuilder : AbstractBuilder
    {
        public CritterBuilder()
        {
            Reset();
        }

        public override void Reset()
        {
            entity = new Critter();
        }

        public override AbstractBuilder AddMovement()
        {
            (entity as Critter).SetMovementStrategy(new CritterMovement(entity));
            return this;
        }

        public CritterBuilder AddSpeech(string speechText, int triggerSize)
        {
            return this;
        }

        public override GameEntity Build()
        {
            Critter finalCritter = entity as Critter;
            Reset();
            return finalCritter;
        }
    }
}
