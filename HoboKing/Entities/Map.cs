using HoboKing.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace HoboKing.Entities
{
    class Map
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int VisibleTilesX { get; set; }
        public int VisibleTilesY { get; set; } 
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }
        public string Level { get; set; }

        readonly Dictionary<char, Tile> tiles = new Dictionary<char, Tile>();

        public Map(int windowWidth, int windowHeight, int tileWidth = 50, int tileHeight = 50,
            int mapWidth = 64, int mapHeight = 54)
        {
            Width = mapWidth;
            Height = mapHeight;
            TileWidth = tileWidth;
            TileHeight = tileHeight;
            VisibleTilesX = windowWidth / tileWidth;
            VisibleTilesY = windowHeight / tileHeight;

            using (var stream = TitleContainer.OpenStream("map.txt"))
            {
                using (var reader = new StreamReader(stream))
                {
                    while (!reader.EndOfStream)
                    {
                        Level += reader.ReadLine();
                    }
                        
                }
            }

        }

        public void Print()
        {
            Console.WriteLine("Map Width: " + Width);
            Console.WriteLine("Map Height: " + Height);
            Console.WriteLine("Tile Width: " + TileWidth);
            Console.WriteLine("Tile Height: " + TileHeight);
            Console.WriteLine("VisibleTilesX: " + VisibleTilesX);
            Console.WriteLine("VisibleTilesY: " + VisibleTilesY);
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
                        case '<':
                            // Draw platform blocks here
                            if (tiles.ContainsKey(TileID))
                            {
                                Tile tile = tiles[TileID];
                                tile.Position = new Vector2(x * TileWidth, y * TileHeight);
                                tile.Draw(spriteBatch, gameTime);
                            }
                            else
                            {
                                throw new Exception($"Tile type ({TileID}) doesn't exist");
                            }
                            break;
                        case '>':
                            // Draw platform blocks here
                            if (tiles.ContainsKey(TileID))
                            {
                                Tile tile = tiles[TileID];
                                tile.Position = new Vector2(x * TileWidth, y * TileHeight);
                                tile.Draw(spriteBatch, gameTime);
                            }
                            else
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
