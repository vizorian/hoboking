using System;
using System.Collections.Generic;
using System.Text;

namespace HoboKing.Iterator
{
    class VerticalIterator : Iterator
    {
        private TileCollection tileCollection;
        private int current = 0;

        public VerticalIterator(TileCollection tileCollection)
        {
            this.tileCollection = tileCollection;
        }
        public override object First()
        {
            throw new NotImplementedException();
        }

        public override object Next()
        {
            throw new NotImplementedException();
        }

        public override bool IsDone()
        {
            throw new NotImplementedException();
        }

        public override object Current()
        {
            return tileCollection.GetItems()[current];
        }
    }
}
