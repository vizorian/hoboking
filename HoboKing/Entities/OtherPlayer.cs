using HoboKing.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HoboKing.Entities
{
    class OtherPlayer : IGameEntity
    {
        public Sprite Sprite { get; set; }
        public Vector2 Position { get; set; }
        public string ConnectionId { get; set; }

        private Map Map;
        public OtherPlayer(Texture2D spriteSheet, Vector2 position, string connectionId, Map map)
        {
            Sprite = new Sprite(spriteSheet, position);
            Position = position;
            ConnectionId = connectionId;
            Map = map;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            float realPosX = Position.X * Map.TileWidth;
            float realPosY = Position.Y * Map.TileWidth;
            Sprite.Draw(spriteBatch, new Vector2(realPosX, realPosY));
        }

        public void Update(GameTime gameTime)
        {
            
        }

        public void UpdatePosition(Vector2 newPosition)
        {
            Position = newPosition;
        }
    }
}
