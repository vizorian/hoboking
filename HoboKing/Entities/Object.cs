using HoboKing.Control;
using HoboKing.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using tainicom.Aether.Physics2D.Dynamics;

namespace HoboKing.Entities
{
    class Object : IGameEntity
    {
        public Sprite Sprite { get; set; }
        private Movement movement;
 
        public Object(){ }

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
        }

        public IGameEntity ShallowCopy()
        {
            return MemberwiseClone() as Object;
        }

        public IGameEntity DeepCopy()
        {
            var clone = MemberwiseClone() as Object;
            clone.Sprite = new Sprite(Sprite.Texture, Sprite.Position, 60);
            return clone;
        }
    }
}
