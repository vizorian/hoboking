using HoboKing.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using tainicom.Aether.Physics2D.Dynamics;

namespace HoboKing.Factory
{
    public abstract class Tile : GameEntity
    {
        protected Tile(Texture2D texture, Vector2 position, int tileSize, World world) : base(texture, position, tileSize)
        {
            CreatePhysicsObjects(world);
        }

        public override GameEntity ShallowCopy()
        {
            return MemberwiseClone() as Tile;
        }

        public override GameEntity DeepCopy()
        {
            var clone = MemberwiseClone() as Tile;
            clone.CreatePhysicsObjects(world);
            return clone;
        }

        public void CreatePhysicsObjects(World w)
        {
            world = w;
            Body = w.CreateBody(Position * PIXEL_TO_UNIT);
            Body.FixedRotation = true;

            Fixture = Body.CreateRectangle(sizeRectangle.Width * PIXEL_TO_UNIT, sizeRectangle.Height * PIXEL_TO_UNIT,
                1f, Vector2.Zero);
            Fixture.Restitution = RESTITUTION;
            Fixture.Friction = FRICTION;
        }
    }
}