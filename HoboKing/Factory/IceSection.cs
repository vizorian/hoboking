using System.Collections.Generic;
using HoboKing.Entities;
using HoboKing.Graphics;

namespace HoboKing.Factory
{
    internal class IceSection : Section
    {
        public IceSection(List<GameEntity> standardTiles, string level, int mapWidth, int mapHeight,
            int sectionStartPosition, int sectionEndPosition) : base(standardTiles, level, mapWidth, mapHeight,
            sectionStartPosition, sectionEndPosition)
        {
        }

        protected sealed override void ReplaceLeft(GameEntity tile)
        {
            tile.ChangeTexture(ContentLoader.IceLeft);
        }

        protected sealed override void ReplaceRight(GameEntity tile)
        {
            tile.ChangeTexture(ContentLoader.IceRight);
        }

        protected sealed override void UpdateTextures(GameEntity specificTile)
        {
            // NW
            if (!hasNorth && hasEast && hasSouth && !hasWest)
            {
                specificTile?.ChangeTexture(ContentLoader.IceNw);
                return;
            }
            // N

            if (!hasNorth && hasEast && hasSouth)
            {
                specificTile?.ChangeTexture(ContentLoader.IceN);
                return;
            }
            // NE

            if (!hasNorth && !hasEast && hasSouth && hasWest)
            {
                specificTile?.ChangeTexture(ContentLoader.IceNe);
                return;
            }
            // W

            if (hasNorth && hasEast && hasSouth && !hasWest)
            {
                specificTile?.ChangeTexture(ContentLoader.IceW);
                return;
            }
            // Center

            if (hasNorth && hasEast && hasSouth)
            {
                // Check for corners
                // Corner NW
                if (!hasNw && hasNe && hasSw && hasSe)
                {
                    specificTile?.ChangeTexture(ContentLoader.IceCornerNw);
                    return;
                }
                // Corner ME

                if (hasNw && !hasNe && hasSw && hasSe)
                {
                    specificTile?.ChangeTexture(ContentLoader.IceCornerNe);
                    return;
                }
                // Corner SW

                if (hasNw && hasNe && !hasSw && hasSe)
                {
                    specificTile?.ChangeTexture(ContentLoader.IceCornerSw);
                    return;
                }
                // Corner SE

                if (hasNw && hasNe && hasSw && !hasSe)
                {
                    specificTile?.ChangeTexture(ContentLoader.IceCornerSe);
                    return;
                }

                // No corner tiles
                specificTile?.ChangeTexture(ContentLoader.IceCenter);
                return;
            }
            // E

            if (hasNorth && !hasEast && hasSouth && hasWest)
            {
                specificTile?.ChangeTexture(ContentLoader.IceE);
                return;
            }
            // SW

            if (hasNorth && hasEast && !hasWest)
            {
                specificTile?.ChangeTexture(ContentLoader.IceSw);
                return;
            }
            // S

            if (hasNorth && hasEast)
            {
                specificTile?.ChangeTexture(ContentLoader.IceS);
                return;
            }
            // SE

            if (hasNorth && !hasSouth && hasWest)
            {
                specificTile?.ChangeTexture(ContentLoader.IceSe);
                return;
            }

            // Default value N for now
            // To dynamically change according to current tile type, make default value a new blank texture and look for it do discern
            specificTile?.ChangeTexture(ContentLoader.IceN);
        }
    }
}