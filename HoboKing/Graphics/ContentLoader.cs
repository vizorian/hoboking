using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HoboKing.Graphics
{
    class ContentLoader
    {
        public Texture2D BatChest { get; }
        public Texture2D TileTexture { get; }
        public Texture2D TileLeftTexture { get; }
        public Texture2D TileRightTexture { get; }
        public SpriteFont Font { get; }
        public ContentLoader(ContentManager contentManager)
        {
            BatChest = contentManager.Load<Texture2D>("batchest");
            TileTexture = contentManager.Load<Texture2D>("ground");
            TileLeftTexture = contentManager.Load<Texture2D>("ground_left");
            TileRightTexture = contentManager.Load<Texture2D>("ground_right");
            Font = contentManager.Load<SpriteFont>("Debug");
        }
    }
}
