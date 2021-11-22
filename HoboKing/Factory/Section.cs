using HoboKing.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace HoboKing.Factory
{
    abstract class Section
    {
        public List<Tile> standardTiles;
        string Level;
        public int MAP_WIDTH;
        public int MAP_HEIGHT;
        public int sectionStartPosition;
        public int sectionEndPosition;
        public Section(List<Tile> standardTiles, string Level, int MAP_WIDTH, int MAP_HEIGHT, int sectionStartPosition, int sectionEndPosition)
        {
            this.standardTiles = standardTiles;
            this.Level = Level;
            this.MAP_WIDTH = MAP_WIDTH;
            this.MAP_HEIGHT = MAP_HEIGHT;
            this.sectionStartPosition = sectionStartPosition;
            this.sectionEndPosition = sectionEndPosition;
        }

        public abstract void UpdateTextures();
        public char GetTile(int x, int y)
        {
            if (x >= 0 && x < MAP_WIDTH && y >= 0 && y < MAP_HEIGHT)
            {
                return Level[y * MAP_WIDTH + x];
            }
            else return ' ';
        }
    }
}
