using HoboKing.Control;
using HoboKing.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoboKing.Entities
{
    class Critter : IGameEntity
    {
        public Sprite Sprite { get; set; }
        private Movement movement;

        public void SetMovementStrategy(Movement movementStrategy)
        {
            movement = movementStrategy;
        }

        public void SetSprite(Sprite sprite)
        {
            Sprite = sprite;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            Sprite.Update();
            movement.AcceptInputs(gameTime);
        }

        public IGameEntity ShallowCopy()
        {
            return MemberwiseClone() as Critter;
        }

        public IGameEntity DeepCopy()
        {
            var clone = MemberwiseClone() as Critter;
            clone.Sprite = new Sprite(Sprite.Texture, Sprite.Position, 60);
            return clone;
        }
    }
}
