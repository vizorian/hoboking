using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using HoboKing.Builder;
using HoboKing.Factory;
using HoboKing.Iterator;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoboKing.Entities
{
    internal class EntityManager
    {
        private static List<GameEntity> Entities = new List<GameEntity>();
        private readonly List<GameEntity> entitiesToAdd = new List<GameEntity>();
        private readonly List<GameEntity> entitiesToRemove = new List<GameEntity>();
        private GameEntityCollection tileCollection = new GameEntityCollection();
        public Player MainPlayer;
        public List<Player> Players = new List<Player>();

        public static IEnumerable<GameEntity> EntitiesNum => new ReadOnlyCollection<GameEntity>(Entities);

        public int PlayerCount { get; set; }

        /// <summary>
        ///     Add entity to an entity list
        /// </summary>
        /// <param name="entity">Entity</param>
        public void AddEntity(GameEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity), "Null cannot be added as an entity");
            if (entity is Player)
            {
                var player = (Player) entity;
                Players.Add(player);
                PlayerCount++;
                // Saves the main player in a variable.
                if (player.IsOtherPlayer == false) 
                    MainPlayer = player;
            }

            if (entity is Tile)
            {
                tileCollection.Add((Tile)entity);
            }
            else
            {
                entitiesToAdd.Add(entity);
            }

        }

        /// <summary>
        ///     Remove entity from an entity list
        /// </summary>
        /// <param name="entity">Entity</param>
        public void RemoveEntity(GameEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity), "Null cannot be removed");

            if (entity is Player)
            {
                Players.Remove((Player) entity);
                PlayerCount--;
            }

            entitiesToRemove.Add(entity);
        }

        /// <summary>
        ///     Updates the entity manager
        /// </summary>
        /// <param name="gameTime">Game time</param>
        public void Update(GameTime gameTime)
        {
            foreach (var entity in Entities)
            {
                if (entity is Player {IsOtherPlayer: false} player) player.Update(gameTime);
                if (entity is Critter) (entity as Critter).Update(gameTime);
            }

            foreach (var entity in entitiesToAdd) Entities.Add(entity);
            entitiesToAdd.Clear();
            foreach (var entity in entitiesToRemove) Entities.Remove(entity);
            entitiesToRemove.Clear();
        }

        /// <summary>
        ///     Draws an entity
        /// </summary>
        /// <param name="spriteBatch">Sprite batch</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            // Horizontalus iteratorius
            //var iterator = tileCollection.CreateHorizontalIterator();

            //var tile = (Tile)iterator.First();
            //var min = Math.Floor(MainPlayer.Position.Y / 1000) * 1000;
            //var max = Math.Ceiling(MainPlayer.Position.Y / 1000) * 1000;


            //for (; !iterator.IsDone(); tile = (Tile)iterator.Next())
            //{
            //    if (tile.Position.Y <= max && tile.Position.Y >= min)
            //    {
            //        Console.WriteLine("Position {0}-{1}", tile.Position.X, tile.Position.Y);
            //        tile.Draw(spriteBatch);
            //    }
            //}
            //tile.Draw(spriteBatch);

            // Vertikalus iteratorius
            var iterator = tileCollection.CreateVerticalIterator();

            var tile = (Tile)iterator.First();
            var min = Math.Floor(MainPlayer.Position.Y / 1000) * 1000;
            var max = Math.Ceiling(MainPlayer.Position.Y / 1000) * 1000;


            for (; !iterator.IsDone(); tile = (Tile)iterator.Next())
            {
                if (tile.Position.Y <= max && tile.Position.Y >= min)
                {
                    //Console.WriteLine("Position {0}-{1}", tile.Position.X, tile.Position.Y);
                    tile.Draw(spriteBatch);
                }
            }
            tile.Draw(spriteBatch);


            //foreach (var tile in tileCollection.GetItems()) tile.Draw(spriteBatch);

            foreach (var entity in Entities) entity.Draw(spriteBatch);
        }

        /// <summary>
        ///     Retrieves all tiles from the entity list
        /// </summary>
        /// <returns>All tiles</returns>
        public List<GameEntity> GetTiles()
        {
            //var tiles = new List<Tile>();
            //foreach (var entity in entitiesToAdd)
            //    if (entity is Tile tile)
            //        tiles.Add(tile);

            return tileCollection.GetItems();
        }

        public static void Reset()
        {
            Entities.Clear();
        }
    }
}