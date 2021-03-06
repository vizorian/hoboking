using HoboKing.Entities;
using Microsoft.Xna.Framework;

namespace HoboKing.Graphics
{
    internal class Camera
    {
        public Matrix Transform { get; private set; }

        public void Follow(GameEntity target)
        {
            var offset = Matrix.CreateTranslation(0, 0, 0);

            if (target.Position.Y >= 0 && target.Position.Y < 1000)
                offset = Matrix.CreateTranslation(0, 0, 0);
            else if (target.Position.Y >= 1000 && target.Position.Y < 2000)
                offset = Matrix.CreateTranslation(0, -1000, 0);
            else if (target.Position.Y >= 2000 && target.Position.Y < 3000)
                offset = Matrix.CreateTranslation(0, -2000, 0);
            //var position = Matrix.CreateTranslation(
            //    -target.Position.X - (target.Rectangle.Width / 2),
            //    -target.Position.Y - (target.Rectangle.Width / 2),
            //    0);
            //var offset = Matrix.CreateTranslation(
            //    HoboKingGame.GAME_WINDOW_WIDTH / 2,
            //    HoboKingGame.GAME_WINDOW_HEIGHT / 2,
            //    0);
            //Transform = position * offset;

            Transform = offset;
        }
    }
}