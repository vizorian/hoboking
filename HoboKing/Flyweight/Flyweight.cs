using System;
using HoboKing.Factory;
using Newtonsoft.Json;

namespace HoboKing.Flyweight
{
    public class Flyweight
    {
        private readonly Tile sharedState;

        public Flyweight(Tile tile)
        {
            sharedState = tile;
        }

        public void Operation(Tile uniqueState)
        {
            var s = JsonConvert.SerializeObject(sharedState);
            var u = JsonConvert.SerializeObject(uniqueState);
            Console.WriteLine($"Flyweight: Displaying shared {s} and unique {u} state.");
        }
    }
}