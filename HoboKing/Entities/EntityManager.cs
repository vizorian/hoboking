using HoboKing.Factory;
using HoboKing.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace HoboKing.Entities
{
    class EntityManager
    {
        readonly static List<GameEntity> _entities = new List<GameEntity>();
        readonly List<GameEntity> _entitiesToAdd = new List<GameEntity>();
        readonly List<GameEntity> _entitiesToRemove = new List<GameEntity>();

        public static IEnumerable<GameEntity> Entities => new ReadOnlyCollection<GameEntity>(_entities);

        public int PlayerCount { get; set; }
        public List<Player> Players = new List<Player>();
        public Player MainPlayer;

        /// <summary>
        /// Add entity to an entity list
        /// </summary>
        /// <param name="entity">Entity</param>
        public void AddEntity(GameEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Null cannot be added as an entity");
            }
            if (entity.GetType() == typeof(Player))
            {
                var player = (Player)entity;
                Players.Add(player);
                PlayerCount++;
                // Saves the main player in a variable.
                if (player.IsOtherPlayer == false)
                {
                    MainPlayer = player;
                }
            }
            _entitiesToAdd.Add(entity);
        }

        /// <summary>
        /// Remove entity from an entity list
        /// </summary>
        /// <param name="entity">Entity</param>
        public void RemoveEntity(GameEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Null cannot be removed");
            }

            if (entity.GetType() == typeof(Player))
            {
                Players.Remove((Player)entity);
                PlayerCount--;
            }
            _entitiesToRemove.Add(entity);
        }

        /// <summary>
        /// Updates the entity manager
        /// </summary>
        /// <param name="gameTime">Game time</param>
        public void Update(GameTime gameTime)
        {
            foreach (var entity in _entities)
            {
                if (entity is Player && (entity as Player).IsOtherPlayer == false)
                {
                    (entity as Player).Update(gameTime);
                }
                if (entity is Critter)
                {
                    (entity as Critter).Update(gameTime);
                }
            }
            foreach (var entity in _entitiesToAdd)
            {
                _entities.Add(entity);
            }
            _entitiesToAdd.Clear();
            foreach (var entity in _entitiesToRemove)
            {
                _entities.Remove(entity);
            }
            _entitiesToRemove.Clear();
        }

        /// <summary>
        /// Draws an entity
        /// </summary>
        /// <param name="spriteBatch">Sprite batch</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(var entity in _entities)
            {
                entity.Draw(spriteBatch);
            }
        }

        /// <summary>
        /// Retrieves all tiles from the entity list
        /// </summary>
        /// <returns>All tiles</returns>
        public List<Tile> GetTiles()
        {
            var tiles = new List<Tile>();
            foreach(var entity in _entitiesToAdd)
            {
                if(entity is Tile)
                {
                    tiles.Add(entity as Tile);
                }
            }
            return tiles;
        }
    }
}
