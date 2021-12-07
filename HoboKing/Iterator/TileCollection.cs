using System;
using System.Collections.Generic;
using System.Text;
using HoboKing.Factory;
using Xunit.Sdk;

namespace HoboKing.Iterator
{
    public class TileCollection : IterableCollection
    {
        List<Tile> tiles = new List<Tile>();

        public override Iterator CreateSectionIterator()
        {
            return new VerticalIterator(this);
        }

        public override Iterator CreateOrderIterator()
        {
            return new HorzontalIterator(this);
        }

        public override Iterator CreateDistanceIterator()
        {
            throw new NotImplementedException();
        }

        public int GetSectionCount()
        {
            var i = 0;
            var maxValueY = 0f;
            while (true)
            {
                if (tiles[i].Position.X == 0)
                    maxValueY = tiles[i].Position.Y;
                else
                    break;

            }
            var sectionCount = Math.Floor(maxValueY / 1000);

            return Convert.ToInt32(sectionCount);
        }

        public List<Tile> GetItems()
        {
            return tiles;
        }

        public void Add(Tile tile)
        {
            tiles.Add(tile);
        }


        public int Count()
        {
            return tiles.Count;
        }
    }
}
