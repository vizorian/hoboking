using System.Collections.Generic;

namespace HoboKing.Factory
{
    internal abstract class Section
    {
        private readonly string level;
        public int MapHeight;
        public int MapWidth;
        public int SectionEndPosition;
        public int SectionStartPosition;
        public List<Tile> StandardTiles;

        protected Section(List<Tile> standardTiles, string level, int mapWidth, int mapHeight, int sectionStartPosition,
            int sectionEndPosition)
        {
            this.StandardTiles = standardTiles;
            this.level = level;
            this.MapWidth = mapWidth;
            this.MapHeight = mapHeight;
            this.SectionStartPosition = sectionStartPosition;
            this.SectionEndPosition = sectionEndPosition;
        }

        public abstract void UpdateTextures();

        public char GetTile(int x, int y)
        {
            if (x >= 0 && x < MapWidth && y >= 0 && y < MapHeight)
                return level[y * MapWidth + x];
            return ' ';
        }
    }
}