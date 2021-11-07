using HoboKing.Entities;
using HoboKing.Factory;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using tainicom.Aether.Physics2D.Dynamics;

namespace HoboKing.Factory
{
    class MapCreator : Creator
    {
        public override Section CreateMapSection(EntityManager EntityManager, string Level, int MAP_WIDTH, int MAP_HEIGHT, int sectionStartPosition, int sectionEndPosition)
        {
            if (sectionStartPosition == 100)
            {
                return new GrassSection(EntityManager, Level, MAP_WIDTH, MAP_HEIGHT, sectionStartPosition, sectionEndPosition);
            }
            else if (sectionStartPosition == 50)
            {
                return new IceSection(EntityManager, Level, MAP_WIDTH, MAP_HEIGHT, sectionStartPosition, sectionEndPosition);
            }
            else if (sectionStartPosition == 0)
            {
                return new SandSection(EntityManager, Level, MAP_WIDTH, MAP_HEIGHT, sectionStartPosition, sectionEndPosition);
            }

            return null;
        }
 
    }
}
