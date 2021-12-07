using System;
using System.Collections.Generic;
using System.Text;
using HoboKing.Entities;
using HoboKing.Factory;
using Xunit.Sdk;

namespace HoboKing.Iterator
{
    public class TileCollection : IterableCollection
    {
        List<GameEntity> collection = new List<GameEntity>();

        public override Iterator CreateVerticalIterator()
        {
            return new VerticalIterator(this);
        }

        public override Iterator CreateHorizontalIterator()
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
                if (collection[i].Position.X == 0)
                    maxValueY = collection[i].Position.Y;
                else
                    break;

            }
            var sectionCount = Math.Floor(maxValueY / 1000);

            return Convert.ToInt32(sectionCount);
        }

        public List<GameEntity> GetItems()
        {
            return collection;
        }

        public void Add(Tile tile)
        {
            collection.Add(tile);
        }


        public int Count()
        {
            return collection.Count;
        }
    }
}
