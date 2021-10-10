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

        public Sprite(Texture2D texture, Vector2 position)
        {
            Texture = texture;
            Position = position;
            Color = Color.White;
            showRectangle = true;
        }

        public Sprite(GraphicsDevice graphics, Texture2D texture, Vector2 position) : this(texture, position)
        {
            SetRectangleTexture(graphics, texture);
        }

        public Sprite(GraphicsDevice graphics, Texture2D texture, Vector2 position, int size) : this(texture, position)
        {
            SetRectangleTexture(graphics, size);
        }

        public void SetShowRectangle(bool show)
        {
            showRectangle = show;
        }

        private void SetRectangleTexture(GraphicsDevice graphics, Texture2D texture)
        {
            var colors = new List<Color>();
            for (int y = 0; y < texture.Height; y++)
            {
                for (int x = 0; x < texture.Width; x++)
                {
                    if (x == 0 || y == 0 || x == texture.Width - 1 || y == texture.Height - 1)
                    {
                        colors.Add(Color.Black);
                    } else
                    {
                        colors.Add(new Color(0, 0, 0, 0));
                    }
                }
            }

            rectangleTexture = new Texture2D(graphics, texture.Width, texture.Height);
            rectangleTexture.SetData<Color>(colors.ToArray());
        }

        private void SetRectangleTexture(GraphicsDevice graphics, int size)
        {
            var colors = new List<Color>();
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    if (x == 0 || y == 0 || x == size - 1 || y == size - 1)
                    {
                        colors.Add(Color.Black);
                    }
                    else
                    {
                        colors.Add(new Color(0, 0, 0, 0));
                    }
                }
            }

            rectangleTexture = new Texture2D(graphics, size, size);
            rectangleTexture.SetData<Color>(colors.ToArray());
        }

        public void Update(GameTime gameTime)
        {

        }

        // Simple drawing method for a sprite
        public virtual void Draw(SpriteBatch spriteBatch, Vector2 drawingPosition)
        {
            spriteBatch.Draw(Texture, drawingPosition, Color);
            if (showRectangle)
            {
                if (rectangleTexture != null)
                {
                    spriteBatch.Draw(rectangleTexture, drawingPosition, Color);
                }
            }
        }

        // Drawing with a size setting
        public virtual void Draw(SpriteBatch spriteBatch, int size)
        {
            spriteBatch.Draw(Texture, new Rectangle((int)Position.X, (int)Position.Y, size, size), Color);
            if (showRectangle)
            {
                if (rectangleTexture != null)
                {
                    spriteBatch.Draw(rectangleTexture, Position, Color);
                }
            }
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
