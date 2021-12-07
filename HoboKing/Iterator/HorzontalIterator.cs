using System;
using System.Collections.Generic;
using System.Text;

namespace HoboKing.Iterator
{
    class HorzontalIterator : Iterator
    {
        private TileCollection tileCollection;
        private int count = 0;
        private int sectionCount;
        float lastX = -1f;
        float currentY = 0f;

        public HorzontalIterator(TileCollection tileCollection)
        {
            this.tileCollection = tileCollection;
            //sectionCount = tileCollection.GetSectionCount();
        }

        public override object First()
        {
            count++;
            return tileCollection.GetItems()[0];
        }

        public override object Next()
        {
            bool found = false;
            if (!IsDone())
            {
                while (!found)
                {
                    for (int i = 1; i < tileCollection.Count(); i++)
                    {

                        if (tileCollection.GetItems()[i].Position.X > lastX &&
                            tileCollection.GetItems()[i].Position.Y == currentY)
                        {
                            found = true; //useless
                            count++;
                            lastX = tileCollection.GetItems()[i].Position.X;
                            return tileCollection.GetItems()[i];
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
            //return count >= 910;
            return count >= tileCollection.Count();
        }

        public override object Current()
        {
            throw new NotImplementedException();
        }
    }
}
