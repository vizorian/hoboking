using System;
using System.Collections.Generic;
using System.Text;

namespace HoboKing.Entities.Factory
{
    class MapCreator : Creator
    {
        public override MapSection CreateMapSection(int level)
        {
            switch (level)
            {
                case 1:
                    return new NormalSection();
                case 2:
                    return new IceSection();
                case 3:
                    return new SlowSection();
                default:
                    return null;
            }

        }

    }
}
