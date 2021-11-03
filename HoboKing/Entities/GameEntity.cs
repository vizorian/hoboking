using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using tainicom.Aether.Physics2D.Dynamics;

namespace HoboKing.Graphics
{
    public abstract class GameEntity
    {
        protected const float FRICTION = 0.3f;
        protected const float RESTITUTION = 0.1f;

        protected const float unitToPixel = 100.0f;
        protected const float pixelToUnit = 1 / unitToPixel;

        public Texture2D Texture { get; protected set; }
        public Vector2 Position { get; set; }

        // Physics
        public Body body;
        protected Fixture fixture;
        protected World world;

        // Rectangle for resizing the texture if necessary
        protected Rectangle Size;
        protected int tileSize;

        public GameEntity() { }

        public GameEntity(Texture2D texture, Vector2 position, int size = 0)
        {
            // Recalculates tiles to absolute coordinates
            float realPosX = position.X * MapComponent.TILE_SIZE;
            float realPosY = position.Y * MapComponent.TILE_SIZE;

            Position = new Vector2(realPosX, realPosY);
            Texture = texture;
            tileSize = size;

            Size = size != 0 ? new Rectangle((int)Position.X, (int)Position.Y, size, size) : new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
        }


        protected virtual void CreatePhysicsObjects(World world, BodyType bodyType)
        {
            this.world = world;
            body = world.CreateBody(Position * pixelToUnit, 0, bodyType);
            body.FixedRotation = true;
            if (bodyType is BodyType.Dynamic)
            {
                fixture = body.CreateCircle(Size.Width / 2f * pixelToUnit, 1f, new Vector2(Size.Width / 2f * pixelToUnit, Size.Height / 2f * pixelToUnit));
            }
            else
            {
                fixture = body.CreateRectangle(Size.Width * pixelToUnit, Size.Height * pixelToUnit, 1f, Vector2.Zero);
            }
            fixture.Restitution = RESTITUTION;
            fixture.Friction = FRICTION;
        }

        public void Update()
        {
            if (body != null)
            {
                Size.X = Convert.ToInt32(body.Position.X * unitToPixel);
                Size.Y = Convert.ToInt32(body.Position.Y * unitToPixel);
                Position = new Vector2(Size.X, Size.Y);
            }
            else
            {
                Size.X = (int)Position.X;
                Size.Y = (int)Position.Y;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
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
            if (body != null)
            {
                body.Position = newPosition * pixelToUnit;
            }
        }

        public void ChangeSize(int newSize)
        {
            tileSize = newSize;
            Size = tileSize != 0 ? new Rectangle((int)Position.X, (int)Position.Y, tileSize, tileSize) : new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
        }

        public void UseGravity(bool useGravity)
        {
            if (body != null)
            {
                body.IgnoreGravity = !useGravity;
            }
        }

        public abstract GameEntity ShallowCopy();

        public abstract GameEntity DeepCopy();
    }
}
