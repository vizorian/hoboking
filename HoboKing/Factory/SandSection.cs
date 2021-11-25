using System.Collections.Generic;
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

        public override void UpdateTextures()
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
                        var hasNorth = false;
                        var hasEast = false;
                        var hasSouth = false;
                        var hasWest = false;
                        var hasNw = false;
                        var hasNe = false;
                        var hasSw = false;
                        var hasSe = false;

                        if (GetTile(x, y - 1) != '.') hasNorth = true;
                        if (GetTile(x + 1, y) != '.') hasEast = true;
                        if (GetTile(x, y + 1) != '.') hasSouth = true;
                        if (GetTile(x - 1, y) != '.') hasWest = true;

                        if (GetTile(x - 1, y - 1) != '.') hasNw = true;
                        if (GetTile(x + 1, y - 1) != '.') hasNe = true;
                        if (GetTile(x - 1, y + 1) != '.') hasSw = true;
                        if (GetTile(x + 1, y + 1) != '.') hasSe = true;

                        // NW
                        if (!hasNorth && hasEast && hasSouth && !hasWest)
                        {
                            specificTile?.ChangeTexture(ContentLoader.SandNw);
                            continue;
                        }
                        // N

                        if (!hasNorth && hasEast && hasSouth)
                        {
                            specificTile?.ChangeTexture(ContentLoader.SandN);
                            continue;
                        }
                        // NE

                        if (!hasNorth && !hasEast && hasSouth && hasWest)
                        {
                            specificTile?.ChangeTexture(ContentLoader.SandNe);
                            continue;
                        }
                        // W

                        if (hasNorth && hasEast && hasSouth && !hasWest)
                        {
                            specificTile?.ChangeTexture(ContentLoader.SandW);
                            continue;
                        }
                        // Center

                        if (hasNorth && hasEast && hasSouth)
                        {
                            // Check for corners
                            // Corner NW
                            if (!hasNw && hasNe && hasSw && hasSe)
                            {
                                specificTile?.ChangeTexture(ContentLoader.SandCornerNw);
                                continue;
                            }
                            // Corner ME

                            if (hasNw && !hasNe && hasSw && hasSe)
                            {
                                specificTile?.ChangeTexture(ContentLoader.SandCornerNe);
                                continue;
                            }
                            // Corner SW

                            if (hasNw && hasNe && !hasSw && hasSe)
                            {
                                specificTile?.ChangeTexture(ContentLoader.SandCornerSw);
                                continue;
                            }
                            // Corner SE

                            if (hasNw && hasNe && hasSw && !hasSe)
                            {
                                specificTile?.ChangeTexture(ContentLoader.SandCornerSe);
                                continue;
                            }

                            // No corner tiles
                            specificTile?.ChangeTexture(ContentLoader.SandCenter);
                            continue;
                        }
                        // E

                        if (hasNorth && !hasEast && hasSouth && hasWest)
                        {
                            specificTile?.ChangeTexture(ContentLoader.SandE);
                            continue;
                        }
                        // SW

                        if (hasNorth && hasEast && !hasWest)
                        {
                            specificTile?.ChangeTexture(ContentLoader.SandSw);
                            continue;
                        }
                        // S

                        if (hasNorth && hasEast)
                        {
                            specificTile?.ChangeTexture(ContentLoader.SandS);
                            continue;
                        }
                        // SE

                        if (hasNorth && !hasSouth && hasWest)
                        {
                            specificTile?.ChangeTexture(ContentLoader.SandSe);
                            continue;
                        }

                        // Default value N for now
                        // To dynamically change according to current tile type, make default value a new blank texture and look for it do discern
                        specificTile?.ChangeTexture(ContentLoader.SandN);
                        break;
                    }
                    case '<':
                        specificTile?.ChangeTexture(ContentLoader.SandLeft);
                        break;
                    case '>':
                        specificTile?.ChangeTexture(ContentLoader.SandRight);
                        break;
                }
            }
        }
    }
}