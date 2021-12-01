using System.Collections.Generic;
using HoboKing.Graphics;

namespace HoboKing.Factory
{
    public abstract class Section
    {
        private readonly string level;

        protected bool hasEast;
        protected bool hasNe;
        protected bool hasNorth;
        protected bool hasNw;
        protected bool hasSe;
        protected bool hasSouth;
        protected bool hasSw;
        protected bool hasWest;

        private readonly int MapHeight;
        private readonly int MapWidth;
        private readonly int SectionEndPosition;
        private readonly int SectionStartPosition;
        private readonly List<Tile> StandardTiles;

        protected Section(List<Tile> standardTiles, string level, int mapWidth, int mapHeight, int sectionStartPosition,
            int sectionEndPosition)
        {
            StandardTiles = standardTiles;
            this.level = level;
            MapWidth = mapWidth;
            MapHeight = mapHeight;
            SectionStartPosition = sectionStartPosition;
            SectionEndPosition = sectionEndPosition;
        }

        public void TemplateMethod()
        {
            for (var x = 0; x < MapWidth; x++)
            for (var y = SectionStartPosition; y < SectionEndPosition; y++)
            {
                var tileId = GetTile(x, y);
                var specificTile = StandardTiles.Find(o => o.Position.X == x * 20 && o.Position.Y == y * 20);
                switch (tileId)
                {
                    case '#':
                    {
                        SetBooleanValues(x, y);
                        UpdateTextures(specificTile);
                        break;
                    }
                    case '<':
                        ReplaceLeft(specificTile);
                        continue;
                    case '>':
                        ReplaceRight(specificTile);
                        continue;
                }
            }
        }

        private char GetTile(int x, int y)
        {
            if (x >= 0 && x < MapWidth && y >= 0 && y < MapHeight)
                return level[y * MapWidth + x];
            return ' ';
        }

        private void SetBooleanValues(int x, int y)
        {
            hasNorth = false;
            hasEast = false;
            hasSouth = false;
            hasWest = false;
            hasNw = false;
            hasNe = false;
            hasSw = false;
            hasSe = false;

            if (GetTile(x, y - 1) != '.') hasNorth = true;
            if (GetTile(x + 1, y) != '.') hasEast = true;
            if (GetTile(x, y + 1) != '.') hasSouth = true;
            if (GetTile(x - 1, y) != '.') hasWest = true;

            if (GetTile(x - 1, y - 1) != '.') hasNw = true;
            if (GetTile(x + 1, y - 1) != '.') hasNe = true;
            if (GetTile(x - 1, y + 1) != '.') hasSw = true;
            if (GetTile(x + 1, y + 1) != '.') hasSe = true;
        }

        protected abstract void ReplaceLeft(Tile tile);

        protected abstract void ReplaceRight(Tile tile);

        protected abstract void UpdateTextures(Tile specificTile);
    }
}