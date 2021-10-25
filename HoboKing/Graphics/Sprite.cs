using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using tainicom.Aether.Physics2D.Dynamics;

namespace HoboKing.Graphics
{
    public class Sprite
    {
        const float FRICTION = 0.3f;
        const float RESTITUTION = 0.1f;

        const float unitToPixel = 100.0f;
        const float pixelToUnit = 1 / unitToPixel;

        public Vector2 Position { get; set; }
        public Texture2D Texture { get; private set; }

        public Body body;
        private Rectangle Size;
        private Fixture fixture;
        private int tileSize;

        // If size = 0, sprite stays default, if size is specified, then the sprite is resized
        public Sprite(Texture2D texture, Vector2 position, World world, int size = 0)
        {
            Texture = texture;
            Position = position;
            tileSize = size;
            Size = size != 0 ? new Rectangle((int)Position.X, (int)Position.Y, size, size) : new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
            CreatePhysicsObjects(world, BodyType.Static);
        }

        public Sprite(Texture2D texture, Vector2 position, World world, BodyType bodyType, int size = 0)
        {
            Texture = texture;
            Position = position;
            tileSize = size;

            Size = size != 0 ? new Rectangle((int)Position.X, (int)Position.Y, size, size) : new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
            CreatePhysicsObjects(world, bodyType);
        }

        // No physics constructor
        public Sprite(Texture2D texture, Vector2 position, int size = 0)
        {
            Texture = texture;
            Position = position;
            tileSize = size;

            Size = size != 0 ? new Rectangle((int)Position.X, (int)Position.Y, size, size) : new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
        }

        private void CreatePhysicsObjects(World world, BodyType bodyType)
        {
            body = world.CreateBody(Position * pixelToUnit, 0, bodyType);
            body.FixedRotation = true;
            if (bodyType is BodyType.Dynamic)
            {
                fixture = body.CreateCircle(Size.Width / 2f * pixelToUnit, 1f, new Vector2(Size.Width / 2f * pixelToUnit, Size.Height / 2f * pixelToUnit));
                fixture.Restitution = RESTITUTION;
                fixture.Friction = FRICTION;
            } else {
                fixture = body.CreateRectangle(Size.Width * pixelToUnit, Size.Height * pixelToUnit, 1f, Vector2.Zero);
                fixture.Restitution = RESTITUTION;
                fixture.Friction = FRICTION;
            }
        }

        public void Update()
        {
            if (body != null)
            {
                Size.X = Convert.ToInt32(body.Position.X * unitToPixel);
                Size.Y = Convert.ToInt32(body.Position.Y * unitToPixel);
                Position = new Vector2(Size.X, Size.Y);
            } else
            {
                Size.X = (int)Position.X;
                Size.Y = (int)Position.Y;
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            Update();
            spriteBatch.Draw(Texture, Size, Color.White);
        }

        public void ChangeTexture(Texture2D texture)
        {
            Texture = texture;
        }

        public void ChangePosition(Vector2 newPosition)
        {
            Position = newPosition;
            Size = tileSize != 0 ? new Rectangle((int)Position.X, (int)Position.Y, tileSize, tileSize) : new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
            body.Position = newPosition * pixelToUnit;
        }

        public void UseGravity(bool useGravity)
        {
            body.IgnoreGravity = !useGravity;
        }
    }
}
