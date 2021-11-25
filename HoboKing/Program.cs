using System;
using System.Diagnostics.CodeAnalysis;

namespace HoboKing
{
    [ExcludeFromCodeCoverage]
    internal static class Program
    {
        [STAThread]
        public static void Main()
        {
            using var game = new HoboKingGame();
            game.Run();
        }
    }
}