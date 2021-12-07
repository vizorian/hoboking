using System;
using System.Collections.Generic;
using System.Text;

namespace HoboKing.Iterator
{
    class HorzontalIterator : Iterator
    {
        private GameEntityCollection collection;
        private int count = 0;
        float lastX = -1f;
        float currentY = 0f;

        public HorzontalIterator(GameEntityCollection tileCollection)
        {
            this.collection = tileCollection;
        }

        public override object First()
        {
            count++;
            return collection.GetItems()[0];
        }

        public override object Next()
        {
            bool found = false;
            if (!IsDone())
            {
                while (!found)
                {
                    for (int i = 1; i < collection.Count(); i++)
                    {

                        if (collection.GetItems()[i].Position.X > lastX &&
                            collection.GetItems()[i].Position.Y == currentY)
                        {
                            found = true;
                            count++;
                            lastX = collection.GetItems()[i].Position.X;
                            return collection.GetItems()[i];
                        }
                    }
                    // jei neranda jokios reiksmes
                    currentY += 20;
                    lastX = -1f;
                }
            }
            return null;
        }

        public override bool IsDone()
        {
            return count >= collection.Count();
        }
    }
}
