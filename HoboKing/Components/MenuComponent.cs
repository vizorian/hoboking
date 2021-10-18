using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace HoboKing.States
{
    public class MenuComponent : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private HoboKingGame hoboKingGame;

        public MenuComponent(HoboKingGame hoboKingGame) : base(hoboKingGame)
        {
            this.hoboKingGame = hoboKingGame;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            //if (hoboKingGame.NewKey(Keys.Enter))
            //    hoboKingGame.SwitchScene(hoboKingGame.levelScene);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
