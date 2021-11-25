using HoboKing.Control.Strategy;
using HoboKing.Entities;

namespace HoboKing.Builder
{
    internal class ObjectBuilder : AbstractBuilder
    {
        public override AbstractBuilder AddMovement()
        {
            (entity as Object)?.SetMovementStrategy(new CritterMovement(entity));
            return this;
        }


        public AbstractBuilder AddSpeech(string speechText, int triggerSize)
        {
            return this;
        }

        public override GameEntity Build()
        {
            var finalEntity = entity as Object;
            Reset();
            return finalEntity;
        }

        public override void Reset()
        {
            entity = new Object();
        }
    }
}