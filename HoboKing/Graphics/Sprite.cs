using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace HoboKing.Graphics
{
    public class Sprite
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public Texture2D Texture { get; private set; }
        public bool Collided { get; private set; }
        public int Size { get; set; }

        private Rectangle rectangle;
        private Texture2D rectangleTexture;
        private Color color;

        private bool showRectangle;

        // If size = 0, sprite stays default, if size is specified, then the sprite is resized
        public Sprite(GraphicsDevice graphics, Texture2D texture, Vector2 position, int size = 0)
        {
            Texture = texture;
            Position = position;
            color = Color.White;
            showRectangle = true;
            Collided = false;
            Size = size;
            if (Size == 0)
            {
                rectangle = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
            } else
            {
                rectangle = new Rectangle((int)Position.X, (int)Position.Y, size, size);
            }
            SetRectangleTexture(graphics);
        }

        public void SetShowRectangle(bool show)
        {
            showRectangle = show;
        }

        private void SetRectangleTexture(GraphicsDevice graphics)
        {
            var colors = new List<Color>();
            for (int y = 0; y < rectangle.Height; y++)
            {
                for (int x = 0; x < rectangle.Width; x++)
                {
                    if (x == 0 || y == 0 || x == rectangle.Width - 1 || y == rectangle.Height - 1)
                    {
                        colors.Add(Color.Black);
                    } else
                    {
                        colors.Add(new Color(0, 0, 0, 0));
                    }
                }
            }

            rectangleTexture = new Texture2D(graphics, rectangle.Width, rectangle.Height);
            rectangleTexture.SetData<Color>(colors.ToArray());
        }

        public void Update(GameTime gameTime)
        {
            rectangle.X = (int)Position.X;
            rectangle.Y = (int)Position.Y;
        }

        // Simple drawing method for a sprite
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (showRectangle) ChangeColorOnCollision();
            if (Size == 0)
            {
                spriteBatch.Draw(Texture, Position, color);
            } else
            {
                spriteBatch.Draw(Texture, new Rectangle((int)Position.X, (int)Position.Y, Size, Size), color);
            }
            if (showRectangle)
            {
                if (rectangleTexture != null)
                {
                    spriteBatch.Draw(rectangleTexture, Position, color);
                }
            }
            color = Color.White;
        }

        private void ChangeColorOnCollision()
        {
            if (Collided)
                color = Color.Red;                
        }

        public bool Collision(Sprite target)
        {
            bool intersects = rectangle.Intersects(target.rectangle);
            Collided = intersects;
            target.Collided = intersects;
            return intersects;
        }
    }
}
