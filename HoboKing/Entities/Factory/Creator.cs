using System;
using System.Collections.Generic;
using System.Text;

namespace HoboKing.Entities.Factory
{
    public abstract class Creator
    {
        public abstract MapSection CreateMapSection(int level);
    }
}
