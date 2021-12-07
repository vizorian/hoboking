using System;
using System.Collections.Generic;
using System.Text;

namespace HoboKing.Iterator
{
    public abstract class IterableCollection
    {
        public abstract Iterator CreateVerticalIterator();
        public abstract Iterator CreateHorizontalIterator();
        public abstract Iterator CreateDistanceIterator();
    }
}
