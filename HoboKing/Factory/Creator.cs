using System.Collections.Generic;

namespace HoboKing.Factory
{
    internal abstract class Creator
    {
        public abstract Section CreateMapSection(List<Tile> standardTiles, string level, int mapWidth, int mapHeight,
            int sectionStartPosition, int sectionEndPosition);
    }
}