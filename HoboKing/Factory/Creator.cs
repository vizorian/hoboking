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
        //public abstract Tile CreateTile(string tileType, Texture2D texture, Vector2 position, int tileSize, World world);
        public abstract Section CreateMapSection(EntityManager EntityManager, string Level, int MAP_WIDTH, int MAP_HEIGHT, int sectionStartPosition, int sectionEndPosition);
    }
}
