using HoboKing.Control.Strategy;
using HoboKing.Entities;

namespace HoboKing.Builder
{
    internal class CritterBuilder : AbstractBuilder
    {
        public CritterBuilder()
        {
            Reset();
        }

        public sealed override void Reset()
        {
            entity = new Critter();
        }

        public override AbstractBuilder AddMovement()
        {
            (entity as Critter)?.SetMovementStrategy(new CritterMovement(entity));
            return this;
        }

        public CritterBuilder AddSpeech(string speechText, int triggerSize)
        {
            return this;
        }

        public override GameEntity Build()
        {
            var finalCritter = entity as Critter;
            Reset();
            return finalCritter;
        }
    }
}