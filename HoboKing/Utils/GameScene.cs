using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace HoboKing.Utils
{
    internal class GameScene
    {
        private readonly List<GameComponent> components;
        private readonly HoboKingGame hoboKingGame;

        public GameScene(HoboKingGame hoboKingGame, params GameComponent[] components)
        {
            this.hoboKingGame = hoboKingGame;
            this.components = new List<GameComponent>();
            foreach (var component in components) AddComponent(component);
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