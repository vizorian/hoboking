using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HoboKing.States
{
    class MenuButtons
    {
        public Vector2 startSingleplayerButtonPosition;
        public Vector2 startMultiplayerButtonPosition;
        public Vector2 optionsButtonPosition;
        public Vector2 returnButtonPosition;
        public Vector2 exitButtonPosition;

        public MenuButtons(GraphicsDevice graphics)
        {
            startSingleplayerButtonPosition = new Vector2(graphics.Viewport.Width / 4, 600);
            this.startMultiplayerButtonPosition = new Vector2(graphics.Viewport.Width / 4, 665);
            this.optionsButtonPosition = new Vector2(graphics.Viewport.Width / 4, 730);
            this.returnButtonPosition = new Vector2(graphics.Viewport.Width / 4, 795);
            this.exitButtonPosition = new Vector2(graphics.Viewport.Width / 4, 795);
        }
    }
}
