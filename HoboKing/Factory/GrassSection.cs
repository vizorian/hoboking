using System.Collections.Generic;
using HoboKing.Graphics;

namespace HoboKing.Factory
{
    internal class GrassSection : Section
    {
        public GrassSection(List<Tile> standardTiles, string level, int mapWidth, int mapHeight,
            int sectionStartPosition, int sectionEndPosition) : base(standardTiles, level, mapWidth, mapHeight,
            sectionStartPosition, sectionEndPosition)
        {
        }

        protected override void ReplaceLeft(Tile tile)
        {
            tile.ChangeTexture(ContentLoader.GrassLeft);
        }

        protected override void ReplaceRight(Tile tile)
        {
            tile.ChangeTexture(ContentLoader.GrassRight);
        }

        protected override void UpdateTextures(Tile specificTile)
        {
            // NW
            if (!hasNorth && hasEast && hasSouth && !hasWest)
            {
                specificTile?.ChangeTexture(ContentLoader.GrassNw);
                return;
            }
            // N

            if (!hasNorth && hasEast && hasSouth)
            {
                specificTile?.ChangeTexture(ContentLoader.GrassN);
                return;
            }
            // NE

            if (!hasNorth && !hasEast && hasSouth && hasWest)
            {
                specificTile?.ChangeTexture(ContentLoader.GrassNe);
                return;
            }
            // W

            if (hasNorth && hasEast && hasSouth && !hasWest)
            {
                specificTile?.ChangeTexture(ContentLoader.GrassW);
                return;
            }
            // Center

            if (hasNorth && hasEast && hasSouth)
            {
                // Check for corners
                // Corner NW
                if (!hasNw && hasNe && hasSw && hasSe)
                {
                    specificTile?.ChangeTexture(ContentLoader.GrassCornerNw);
                    return;
                }
                // Corner ME

                if (hasNw && !hasNe && hasSw && hasSe)
                {
                    specificTile?.ChangeTexture(ContentLoader.GrassCornerNe);
                    return;
                }
                // Corner SW

                if (hasNw && hasNe && !hasSw && hasSe)
                {
                    specificTile?.ChangeTexture(ContentLoader.GrassCornerSw);
                    return;
                }
                // Corner SE

                if (hasNw && hasNe && hasSw && !hasSe)
                {
                    specificTile?.ChangeTexture(ContentLoader.GrassCornerSe);
                    return;
                }

                // No corner tiles
                specificTile?.ChangeTexture(ContentLoader.GrassCenter);
                return;
            }
            // E

            if (hasNorth && !hasEast && hasSouth && hasWest)
            {
                specificTile?.ChangeTexture(ContentLoader.GrassE);
                return;
            }
            // SW

            if (hasNorth && hasEast && !hasWest)
            {
                specificTile?.ChangeTexture(ContentLoader.GrassSw);
                return;
            }
            // S

            if (hasNorth && hasEast)
            {
                specificTile?.ChangeTexture(ContentLoader.GrassS);
                return;
            }
            // SE

            if (hasNorth && !hasSouth && hasWest)
            {
                specificTile?.ChangeTexture(ContentLoader.GrassSe);
                return;
            }

            // Default value N for now
            // To dynamically change according to current tile type, make default value a new blank texture and look for it do discern
            specificTile?.ChangeTexture(ContentLoader.GrassN);
        }
    }
}