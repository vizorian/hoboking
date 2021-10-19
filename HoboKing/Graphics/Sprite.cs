using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using tainicom.Aether.Physics2D.Diagnostics;
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

        // If size = 0, sprite stays default, if size is specified, then the sprite is resized
        public Sprite(Texture2D texture, Vector2 position, World world, int size = 0)
        {
            Texture = texture;
            Position = position;
            Size = size != 0 ? new Rectangle((int)Position.X, (int)Position.Y, size, size) : new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
            CreatePhysicsObjects(world, BodyType.Static);
        }

        public Sprite(Texture2D texture, Vector2 position, World world, BodyType bodyType, int size = 0)
        {
            Texture = texture;
            Position = position;
            Size = size != 0 ? new Rectangle((int)Position.X, (int)Position.Y, size, size) : new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
            CreatePhysicsObjects(world, bodyType);
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

        public void DrawDebug(DebugView debugView, GraphicsDevice graphics, ContentManager content)
        {
            //debugView.LoadContent(graphics, content);
            //Matrix projection = Matrix.CreateOrthographic(500, 500, 0, 0);
            //debugView.BeginCustomDraw(projection, Matrix.view)
            //debugView.RenderDebugData(ref projection);
            //debugView.DrawShape(fixture, body.GetTransform(), Color.Black);
        }

        public void Update(GameTime gameTime)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            Size.X = Convert.ToInt32(body.Position.X * unitToPixel);
            Size.Y = Convert.ToInt32(body.Position.Y * unitToPixel);
            spriteBatch.Draw(Texture, Size, Color.White);
        }

        public void ChangeTexture(Texture2D texture)
        {
            Texture = texture;
        }
    }
}
