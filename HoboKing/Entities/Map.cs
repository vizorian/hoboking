using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace HoboKing.Entities
{
    class Map : IGameEntity
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int VisibleTilesX { get; set; }
        public int VisibleTilesY { get; set; }
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }
        public string Level { get; set; }
        public int DrawOrder { get; set; }

        Dictionary<char, Tile> tiles = new Dictionary<char, Tile>();

        public Map(int windowWidth, int windowHeight, int tileWidth = 50, int tileHeight = 50,
            int mapWidth = 16, int mapHeight = 10)
        {
            Width = mapWidth;
            Height = mapHeight;
            TileWidth = tileWidth;
            TileHeight = tileHeight;
            VisibleTilesX = windowWidth / tileWidth;
            VisibleTilesY = windowHeight / tileHeight;

            DrawOrder = 0;

            Level += "####............";
            Level += "#...............";
            Level += "#...............";
            Level += "#...............";
            Level += "#...............";
            Level += "#...........####";
            Level += "####...........#";
            Level += "...............#";
            Level += "...............#";
            Level += "################";
        }

        public void Print()
        {
            Debug.WriteLine("Map Width: " + Width);
            Debug.WriteLine("Map Height: " + Height);
            Debug.WriteLine("Tile Width: " + TileWidth);
            Debug.WriteLine("Tile Height: " + TileHeight);
            Debug.WriteLine("VisibleTilesX: " + VisibleTilesX);
            Debug.WriteLine("VisibleTilesY: " + VisibleTilesY);
        }

        public char GetTile(int x, int y)
        {
            if (x >= 0 && x < Width && y >= 0 && y < Height)
            {
                return Level[y * Width + x];
            }
            else return ' ';
        }

        public string SetTile(int x, int y, char newTile)
        {
            StringBuilder sb = new StringBuilder(Level);
            if (x >= 0 && x < Width && y >= 0 && y < Height)
            {
                sb[y * Width + x] = newTile;
                return sb.ToString();
            } else
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        public void AddTileType(char key, Texture2D texture)
        {
            tiles.Add(key, new Tile(texture, TileWidth));
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            for (int x = 0; x < VisibleTilesX; x++)
            {
                for (int y = 0; y < VisibleTilesY; y++)
                {
                    char TileID = GetTile(x, y);
                    switch (TileID)
                    {
                        case '.':
                            // Empty blocks (background tiles)
                            break;
                        case '#':
                            // Draw platform blocks here
                            if (tiles.ContainsKey(TileID))
                            {
                                Tile tile = tiles[TileID];
                                tile.Position = new Vector2(x * TileWidth, y * TileHeight);
                                tile.Draw(spriteBatch, gameTime);
                            } else
                            {
                                throw new Exception($"Tile type ({TileID}) doesn't exist");
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
