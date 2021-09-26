using System;

namespace HoboKing
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new HoboKingGame())
                game.Run();
        }
    }
}
