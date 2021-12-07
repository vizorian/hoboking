﻿using System;
using System.Collections.Generic;
using System.Text;

namespace HoboKing.Iterator
{
    class VerticalIterator : Iterator
    {
        private GameEntityCollection collection;
        private int position = 0;

        public VerticalIterator(GameEntityCollection tileCollection)
        {
            this.collection = tileCollection;
        }

        public override object First()
        {
            return collection.GetItems()[position];
        }

        public override object Next()
        {
            if (!IsDone())
            {
                position++;
                return collection.GetItems()[position];
            }
            return null;
        }

        public override bool IsDone()
        {
            return position + 1 >= collection.Count();
        }
    }
}
