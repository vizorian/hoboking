﻿using System.Collections.Generic;
using HoboKing.Graphics;

namespace HoboKing.Factory
{
    internal class SandSection : Section
    {
        public SandSection(List<Tile> standardTiles, string level, int mapWidth, int mapHeight,
            int sectionStartPosition, int sectionEndPosition) : base(standardTiles, level, mapWidth, mapHeight,
            sectionStartPosition, sectionEndPosition)
        {
        }

        protected override void ReplaceLeft(Tile tile)
        {
            tile.ChangeTexture(ContentLoader.SandLeft);
        }

        protected override void ReplaceRight(Tile tile)
        {
            tile.ChangeTexture(ContentLoader.SandRight);
        }

        protected override void UpdateTextures(Tile specificTile)
        {
            // NW
            if (!hasNorth && hasEast && hasSouth && !hasWest)
            {
                specificTile?.ChangeTexture(ContentLoader.SandNw);
                return;
            }

            // N
            if (!hasNorth && hasEast && hasSouth)
            {
                specificTile?.ChangeTexture(ContentLoader.SandN);
                return;
            }

            // NE
            if (!hasNorth && !hasEast && hasSouth && hasWest)
            {
                specificTile?.ChangeTexture(ContentLoader.SandNe);
                return;
            }

            // W
            if (hasNorth && hasEast && hasSouth && !hasWest)
            {
                specificTile?.ChangeTexture(ContentLoader.SandW);
                return;
            }
            // Center

            if (hasNorth && hasEast && hasSouth)
            {
                // Check for corners
                // Corner NW
                if (!hasNw && hasNe && hasSw && hasSe)
                {
                    specificTile?.ChangeTexture(ContentLoader.SandCornerNw);
                    return;
                }
                // Corner ME

                if (hasNw && !hasNe && hasSw && hasSe)
                {
                    specificTile?.ChangeTexture(ContentLoader.SandCornerNe);
                    return;
                }
                // Corner SW

                if (hasNw && hasNe && !hasSw && hasSe)
                {
                    specificTile?.ChangeTexture(ContentLoader.SandCornerSw);
                    return;
                }
                // Corner SE

                if (hasNw && hasNe && hasSw && !hasSe)
                {
                    specificTile?.ChangeTexture(ContentLoader.SandCornerSe);
                    return;
                }

                // No corner tiles
                specificTile?.ChangeTexture(ContentLoader.SandCenter);
                return;
            }
            // E

            if (hasNorth && !hasEast && hasSouth && hasWest)
            {
                specificTile?.ChangeTexture(ContentLoader.SandE);
                return;
            }
            // SW

            if (hasNorth && hasEast && !hasWest)
            {
                specificTile?.ChangeTexture(ContentLoader.SandSw);
                return;
            }
            // S

            if (hasNorth && hasEast)
            {
                specificTile?.ChangeTexture(ContentLoader.SandS);
                return;
            }
            // SE

            if (hasNorth && !hasSouth && hasWest)
            {
                specificTile?.ChangeTexture(ContentLoader.SandSe);
                return;
            }

            // Default value N for now
            // To dynamically change according to current tile type, make default value a new blank texture and look for it do discern
            specificTile?.ChangeTexture(ContentLoader.SandN);
        }

    }
}