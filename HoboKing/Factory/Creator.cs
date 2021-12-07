using System.Collections.Generic;
using HoboKing.Entities;

namespace HoboKing.Factory
{
    internal abstract class Creator
    {
        public abstract Section CreateMapSection(List<GameEntity> standardTiles, string level, int mapWidth, int mapHeight,
            int sectionStartPosition, int sectionEndPosition);
    }
}