using HoboKing.Entities;
using HoboKing.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HoboKing.Factory
{
    class IceSection : Section
    {
        public IceSection(EntityManager EntityManager, string Level, int MAP_WIDTH, int MAP_HEIGHT, int sectionStartPosition, int sectionEndPosition) : base(EntityManager, Level, MAP_WIDTH, MAP_HEIGHT, sectionStartPosition, sectionEndPosition)
        {
        }

        public override void UpdateTextures()
        {
            List<Tile> standardTiles = EntityManager.GetTiles();
            // Tile specificTile = standardTiles.Find(o => o.Sprite.Position.X == 2 && o.Sprite.Position.Y == 2);
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
                            specificTile.ChangeTexture(ContentLoader.IceNW); continue;
                        }
                        // N
                        else if (!hasNorth && hasEast && hasSouth && hasWest)
                        {
                            specificTile.ChangeTexture(ContentLoader.IceN); continue;
                        }
                        // NE
                        else if (!hasNorth && !hasEast && hasSouth && hasWest)
                        {
                            specificTile.ChangeTexture(ContentLoader.IceNE); continue;
                        }
                        // W
                        else if (hasNorth && hasEast && hasSouth && !hasWest)
                        {
                            specificTile.ChangeTexture(ContentLoader.IceW); continue;
                        }
                        // Center
                        else if (hasNorth && hasEast && hasSouth && hasWest)
                        {
                            // Check for corners
                            // Corner NW
                            if (!hasNW && hasNE && hasSW && hasSE)
                            {
                                specificTile.ChangeTexture(ContentLoader.IceCornerNW); continue;
                            }
                            // Corner ME
                            else if (hasNW && !hasNE && hasSW && hasSE)
                            {
                                specificTile.ChangeTexture(ContentLoader.IceCornerNE); continue;
                            }
                            // Corner SW
                            else if (hasNW && hasNE && !hasSW && hasSE)
                            {
                                specificTile.ChangeTexture(ContentLoader.IceCornerSW); continue;
                            }
                            // Corner SE
                            else if (hasNW && hasNE && hasSW && !hasSE)
                            {
                                specificTile.ChangeTexture(ContentLoader.IceCornerSE); continue;
                            }
                            else
                            {
                                // No corner tiles
                                specificTile.ChangeTexture(ContentLoader.IceCenter); continue;
                            }
                        }
                        // E
                        else if (hasNorth && !hasEast && hasSouth && hasWest)
                        {
                            specificTile.ChangeTexture(ContentLoader.IceE); continue;
                        }
                        // SW
                        else if (hasNorth && hasEast && !hasSouth && !hasWest)
                        {
                            specificTile.ChangeTexture(ContentLoader.IceSW); continue;
                        }
                        // S
                        else if (hasNorth && hasEast && !hasSouth && hasWest)
                        {
                            specificTile.ChangeTexture(ContentLoader.IceS); continue;
                        }
                        // SE
                        else if (hasNorth && !hasEast && !hasSouth && hasWest)
                        {
                            specificTile.ChangeTexture(ContentLoader.IceSE); continue;
                        }
                        else
                        {
                            // Default value N for now
                            // To dynamically change according to current tile type, make default value a new blank texture and look for it do discern
                            specificTile.ChangeTexture(ContentLoader.IceN);
                        }
                    }
                    if (TileID == '<')
                    {
                        specificTile.ChangeTexture(ContentLoader.IceLeft);
                    }
                    if (TileID == '>')
                    {
                        specificTile.ChangeTexture(ContentLoader.IceRight);
                    }
                }
            }
        }
    }
}
