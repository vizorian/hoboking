using HoboKing.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using tainicom.Aether.Physics2D.Dynamics;

namespace HoboKing.Factory
{
    abstract class Creator
    {
        public abstract Section CreateMapSection(List<Tile> standardTiles, string Level, int MAP_WIDTH, int MAP_HEIGHT, int sectionStartPosition, int sectionEndPosition);
    }
}
