using System.Collections.Generic;
using HoboKing.Entities;

namespace HoboKing.Factory
{
    internal class MapCreator : Creator
    {
        public override Section CreateMapSection(List<GameEntity> standardTiles, string level, int mapWidth, int mapHeight,
            int sectionStartPosition, int sectionEndPosition)
        {
            return sectionStartPosition switch
            {
                100 => new GrassSection(standardTiles, level, mapWidth, mapHeight, sectionStartPosition,
                    sectionEndPosition),
                50 => new IceSection(standardTiles, level, mapWidth, mapHeight, sectionStartPosition,
                    sectionEndPosition),
                0 => new SandSection(standardTiles, level, mapWidth, mapHeight, sectionStartPosition,
                    sectionEndPosition),
                _ => null
            };
        }
    }
}