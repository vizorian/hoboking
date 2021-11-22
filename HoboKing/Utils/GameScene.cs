using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace HoboKing.Scenes
{
    class GameScene
    {
        private List<GameComponent> components;
        private HoboKingGame hoboKingGame;

        public GameScene(HoboKingGame hoboKingGame, params GameComponent[] components)
        {
            this.hoboKingGame = hoboKingGame;
            this.components = new List<GameComponent>();
            foreach (GameComponent component in components)
            {
                AddComponent(component);
            }
        }

        public void AddComponent(GameComponent component)
        {
            components.Add(component);
            if (!hoboKingGame.Components.Contains(component))
                hoboKingGame.Components.Add(component);
        }

        public GameComponent[] ReturnComponents()
        {
            return components.ToArray();
        }
    }
}
