using System;
using System.Collections.Generic;
using System.Text;

namespace HoboKing.Iterator
{
    public abstract class IterableCollection
    {
        public abstract Iterator CreateSectionIterator();
        public abstract Iterator CreateOrderIterator();
        public abstract Iterator CreateDistanceIterator();
    }
}
