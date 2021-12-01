using System;
using System.Collections.Generic;
using System.Linq;
using HoboKing.Factory;
using Microsoft.Xna.Framework.Graphics;

namespace HoboKing.Flyweight
{
    // The Flyweight Factory creates and manages the Flyweight objects.
    // It ensures that flyweights are shared correctly. When the client requests a flyweight,
    // the factory either returns an existing instance or
    // creates a new one, if it doesn't exist yet.
    public class FlyweightFactory
    {
        private readonly List<Tuple<Flyweight, string>> flyweights = new List<Tuple<Flyweight, string>>();

        public FlyweightFactory(params Tile[] args)
        {
            foreach (var elem in args)
            {
                flyweights.Add(new Tuple<Flyweight, string>(new Flyweight(elem), this.GetKey(elem)));
            }
        }

        // Returns a Flyweight's string hash for a given state.
        public string GetKey(Tile key)
        {
            var elements = new List<string>
            {
                key.Texture.Name,
                key.TileSize.ToString(),
                key.world.ToString()
            };

            elements.Sort();

            return string.Join("_", elements);
        }

        // Returns an existing Flyweight with a given state or creates a new one.
        public Flyweight GetFlyweight(Tile sharedState)
        {
            var key = GetKey(sharedState);

            if (flyweights.All(t => t.Item2 != key))
            {
                Console.WriteLine("FlyweightFactory: Can't find a flyweight, creating new one.");
                flyweights.Add(new Tuple<Flyweight, string>(new Flyweight(sharedState), key));
            }
            else
            {
                Console.WriteLine("FlyweightFactory: Reusing existing flyweight.");
            }
            return flyweights.FirstOrDefault(t => t.Item2 == key)?.Item1;
        }

        public void ListFlyweights()
        {
            var count = flyweights.Count;
            Console.WriteLine($"\nFlyweightFactory: I have {count} flyweights:");
            foreach (var flyweight in flyweights)
            {
                Console.WriteLine(flyweight.Item2);
            }
        }
    }
}