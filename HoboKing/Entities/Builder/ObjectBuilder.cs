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
    class ObjectBuilder : AbstractBuilder
    {
        public override AbstractBuilder AddMovement()
        {
            (entity as Object).SetMovementStrategy(new CritterMovement(entity));
            return this;
        }


        public AbstractBuilder AddSpeech(string speechText, int triggerSize)
        {
            return this;
        }

        public override GameEntity Build()
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
