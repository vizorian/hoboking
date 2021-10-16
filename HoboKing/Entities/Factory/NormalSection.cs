using System;
using System.Collections.Generic;
using System.Text;

namespace HoboKing.Entities.Factory
{
    class NormalSection : MapSection
    {
        public override AbstractTileFactory GetAbstractTileFactory()
        {
            return new NormalTileFactory();
        }
    }
}
