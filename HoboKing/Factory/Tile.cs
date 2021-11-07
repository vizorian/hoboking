using HoboKing.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using tainicom.Aether.Physics2D.Dynamics;

namespace HoboKing.Factory
{
    public abstract class Tile : GameEntity
    {
        public Tile(Texture2D texture, Vector2 position, int tileSize, World world) : base(texture, position, tileSize)
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

        public void CreatePhysicsObjects(World world)
        {
            this.world = world;
            body = world.CreateBody(Position * pixelToUnit, 0, BodyType.Static);
            body.FixedRotation = true;

            fixture = body.CreateRectangle(Size.Width * pixelToUnit, Size.Height * pixelToUnit, 1f, Vector2.Zero);
            fixture.Restitution = RESTITUTION;
            fixture.Friction = FRICTION;
        }
    }
}
