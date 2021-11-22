using HoboKing.Entities;
using HoboKing.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HoboKing.Factory
{
    class SandSection : Section
    {
        public SandSection(List<Tile> standardTiles, string Level, int MAP_WIDTH, int MAP_HEIGHT, int sectionStartPosition, int sectionEndPosition) : base(standardTiles, Level, MAP_WIDTH, MAP_HEIGHT, sectionStartPosition, sectionEndPosition)
        {
        }

        public override void UpdateTextures()
        {
            for (int x = 0; x < MAP_WIDTH; x++)
            {
                for (int y = sectionStartPosition; y < sectionEndPosition; y++)
                {
                    char TileID = GetTile(x, y);
                    Tile specificTile = standardTiles.Find(o => o.Position.X == x * 20 && o.Position.Y == y * 20);
                    if (TileID == '#')
                    {
                        bool hasNorth = false;
                        bool hasEast = false;
                        bool hasSouth = false;
                        bool hasWest = false;
                        bool hasNW = false;
                        bool hasNE = false;
                        bool hasSW = false;
                        bool hasSE = false;

                        if (GetTile(x, y - 1) != '.') hasNorth = true;
                        if (GetTile(x + 1, y) != '.') hasEast = true;
                        if (GetTile(x, y + 1) != '.') hasSouth = true;
                        if (GetTile(x - 1, y) != '.') hasWest = true;

                        if (GetTile(x - 1, y - 1) != '.') hasNW = true;
                        if (GetTile(x + 1, y - 1) != '.') hasNE = true;
                        if (GetTile(x - 1, y + 1) != '.') hasSW = true;
                        if (GetTile(x + 1, y + 1) != '.') hasSE = true;

                        // NW
                        if (!hasNorth && hasEast && hasSouth && !hasWest)
                        {
                            specificTile.ChangeTexture(ContentLoader.SandNW); continue;
                        }
                        // N
                        else if (!hasNorth && hasEast && hasSouth && hasWest)
                        {
                            specificTile.ChangeTexture(ContentLoader.SandN); continue;
                        }
                        // NE
                        else if (!hasNorth && !hasEast && hasSouth && hasWest)
                        {
                            specificTile.ChangeTexture(ContentLoader.SandNE); continue;
                        }
                        // W
                        else if (hasNorth && hasEast && hasSouth && !hasWest)
                        {
                            specificTile.ChangeTexture(ContentLoader.SandW); continue;
                        }
                        // Center
                        else if (hasNorth && hasEast && hasSouth && hasWest)
                        {
                            // Check for corners
                            // Corner NW
                            if (!hasNW && hasNE && hasSW && hasSE)
                            {
                                specificTile.ChangeTexture(ContentLoader.SandCornerNW); continue;
                            }
                            // Corner ME
                            else if (hasNW && !hasNE && hasSW && hasSE)
                            {
                                specificTile.ChangeTexture(ContentLoader.SandCornerNE); continue;
                            }
                            // Corner SW
                            else if (hasNW && hasNE && !hasSW && hasSE)
                            {
                                specificTile.ChangeTexture(ContentLoader.SandCornerSW); continue;
                            }
                            // Corner SE
                            else if (hasNW && hasNE && hasSW && !hasSE)
                            {
                                specificTile.ChangeTexture(ContentLoader.SandCornerSE); continue;
                            }
                            else
                            {
                                // No corner tiles
                                specificTile.ChangeTexture(ContentLoader.SandCenter); continue;
                            }
                        }
                        // E
                        else if (hasNorth && !hasEast && hasSouth && hasWest)
                        {
                            specificTile.ChangeTexture(ContentLoader.SandE); continue;
                        }
                        // SW
                        else if (hasNorth && hasEast && !hasSouth && !hasWest)
                        {
                            specificTile.ChangeTexture(ContentLoader.SandSW); continue;
                        }
                        // S
                        else if (hasNorth && hasEast && !hasSouth && hasWest)
                        {
                            specificTile.ChangeTexture(ContentLoader.SandS); continue;
                        }
                        // SE
                        else if (hasNorth && !hasEast && !hasSouth && hasWest)
                        {
                            specificTile.ChangeTexture(ContentLoader.SandSE); continue;
                        }
                        else
                        {
                            // Default value N for now
                            // To dynamically change according to current tile type, make default value a new blank texture and look for it do discern
                            specificTile.ChangeTexture(ContentLoader.SandN);
                        }
                    }
                    if (TileID == '<')
                    {
                        specificTile.ChangeTexture(ContentLoader.SandLeft);
                    }
                    if (TileID == '>')
                    {
                        specificTile.ChangeTexture(ContentLoader.SandRight);
                    }
                }
            }
        }
    }
}
