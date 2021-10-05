using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HoboKing.Graphics
{
    public class Sprite
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public Texture2D Texture { get; private set; }
        public bool Collided { get; private set; }

        protected Rectangle Rectangle;
        private Color Color;
        private string assetName;
        public Sprite(Vector2 position, string textureAssetName)
        {
            Texture = null;
            Position = position;
            Color = Color.White;
            assetName = textureAssetName;
        }

        public void LoadContent(ContentManager contentManager)
        {
            Texture = contentManager.Load<Texture2D>(assetName);
        }

        protected virtual void OnContentLoaded(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            UpdateRectangle();
        }

        private void UpdateRectangle()
        {
            Rectangle = new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
        }

        public void Update(GameTime gameTime)
        {
            Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            UpdateRectangle();
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color);
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
