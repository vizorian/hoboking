using HoboKing.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;


namespace HoboKing.Components
{
    class ConnectorComponent : Microsoft.Xna.Framework.GameComponent
    {
        private HoboKingGame hoboKingGame;
        private Connector connector;
        private Player player;
        
        private float timer = 0.1f;
        const float TIMER = 0.1f;


        public ConnectorComponent(HoboKingGame hoboKingGame) : base(hoboKingGame)
        {
            this.hoboKingGame = hoboKingGame;
        }

        public override void Initialize()
        {
            connector = Connector.getInstance();
            connector.Connect();
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        // Send data every time interval
        private void SendData(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            timer -= elapsed;
            if (timer < 0)
            {
                connector.SendCoordinates(player.Sprite.Position);
                timer = TIMER;
            }
        }
    }
}
