using System.Collections.Generic;
using HoboKing.Graphics;

namespace HoboKing.Factory
{
    internal class IceSection : Section
    {
        public IceSection(List<Tile> standardTiles, string level, int mapWidth, int mapHeight,
            int sectionStartPosition, int sectionEndPosition) : base(standardTiles, level, mapWidth, mapHeight,
            sectionStartPosition, sectionEndPosition)
        {
        }

        public override void UpdateTextures()
        {
            // Tile specificTile = standardTiles.Find(o => o.Sprite.Position.X == 2 && o.Sprite.Position.Y == 2);
            for (var x = 0; x < MapWidth; x++)
            for (var y = SectionStartPosition; y < SectionEndPosition; y++)
            {
                var tileId = GetTile(x, y);
                var specificTile = StandardTiles.Find(o => o.Position.X == x * 20 && o.Position.Y == y * 20);
                if (tileId == '#')
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
                        specificTile?.ChangeTexture(ContentLoader.IceNw);
                        continue;
                    }
                    // N

                    if (!hasNorth && hasEast && hasSouth)
                    {
                        specificTile?.ChangeTexture(ContentLoader.IceN);
                        continue;
                    }
                    // NE

                    if (!hasNorth && !hasEast && hasSouth && hasWest)
                    {
                        specificTile?.ChangeTexture(ContentLoader.IceNe);
                        continue;
                    }
                    // W

                    if (hasNorth && hasEast && hasSouth && !hasWest)
                    {
                        specificTile?.ChangeTexture(ContentLoader.IceW);
                        continue;
                    }
                    // Center

                    if (hasNorth && hasEast && hasSouth)
                    {
                        // Check for corners
                        // Corner NW
                        if (!hasNw && hasNe && hasSw && hasSe)
                        {
                            specificTile?.ChangeTexture(ContentLoader.IceCornerNw);
                            continue;
                        }
                        // Corner ME

                        if (hasNw && !hasNe && hasSw && hasSe)
                        {
                            specificTile?.ChangeTexture(ContentLoader.IceCornerNe);
                            continue;
                        }
                        // Corner SW

                        if (hasNw && hasNe && !hasSw && hasSe)
                        {
                            specificTile?.ChangeTexture(ContentLoader.IceCornerSw);
                            continue;
                        }
                        // Corner SE

                        if (hasNw && hasNe && hasSw && !hasSe)
                        {
                            specificTile?.ChangeTexture(ContentLoader.IceCornerSe);
                            continue;
                        }

                        // No corner tiles
                        specificTile?.ChangeTexture(ContentLoader.IceCenter);
                        continue;
                    }
                    // E

                    if (hasNorth && !hasEast && hasSouth && hasWest)
                    {
                        specificTile?.ChangeTexture(ContentLoader.IceE);
                        continue;
                    }
                    // SW

                    if (hasNorth && hasEast && !hasWest)
                    {
                        specificTile?.ChangeTexture(ContentLoader.IceSw);
                        continue;
                    }
                    // S

                    if (hasNorth && hasEast)
                    {
                        specificTile?.ChangeTexture(ContentLoader.IceS);
                        continue;
                    }
                    // SE

                    if (hasNorth && !hasSouth && hasWest)
                    {
                        specificTile?.ChangeTexture(ContentLoader.IceSe);
                        continue;
                    }

                    // Default value N for now
                    // To dynamically change according to current tile type, make default value a new blank texture and look for it do discern
                    specificTile?.ChangeTexture(ContentLoader.IceN);
                }

                if (tileId == '<') specificTile?.ChangeTexture(ContentLoader.IceLeft);
                if (tileId == '>') specificTile?.ChangeTexture(ContentLoader.IceRight);
            }
        }
    }
}