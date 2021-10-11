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

        private Rectangle Rectangle;
        private Texture2D rectangleTexture;
        private Color Color;

        private bool showRectangle;
        private int Size;

        // If size = 0, sprite stays default, if size is specified, then the sprite is resized
        public Sprite(GraphicsDevice graphics, Texture2D texture, Vector2 position, int size = 0)
        {
            Texture = texture;
            Position = position;
            Color = Color.White;
            showRectangle = true;
            Collided = false;
            Size = size;
            if (Size == 0)
            {
                Rectangle = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
            } else
            {
                Rectangle = new Rectangle((int)Position.X, (int)Position.Y, size, size);
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
            for (int y = 0; y < Rectangle.Height; y++)
            {
                for (int x = 0; x < Rectangle.Width; x++)
                {
                    if (x == 0 || y == 0 || x == Rectangle.Width - 1 || y == Rectangle.Height - 1)
                    {
                        colors.Add(Color.Black);
                    } else
                    {
                        colors.Add(new Color(0, 0, 0, 0));
                    }
                }
            }

            rectangleTexture = new Texture2D(graphics, Rectangle.Width, Rectangle.Height);
            rectangleTexture.SetData<Color>(colors.ToArray());
        }

        public void Update(GameTime gameTime)
        {
            Rectangle.X = (int)Position.X;
            Rectangle.Y = (int)Position.Y;
        }

        // Simple drawing method for a sprite
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (showRectangle) ChangeColorOnCollision();
            if (Size == 0)
            {
                spriteBatch.Draw(Texture, Position, Color);
            } else
            {
                spriteBatch.Draw(Texture, new Rectangle((int)Position.X, (int)Position.Y, Size, Size), Color);
            }
            if (showRectangle)
            {
                if (rectangleTexture != null)
                {
                    spriteBatch.Draw(rectangleTexture, Position, Color);
                }
            }
            Color = Color.White;
        }

        private void ChangeColorOnCollision()
        {
            if (Collided)
                Color = Color.Red;                
        }

        public bool Collision(Sprite target)
        {
            bool intersects = Rectangle.Intersects(target.Rectangle);
            Collided = intersects;
            target.Collided = intersects;
            return intersects;
        }
    }
}
