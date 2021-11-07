using HoboKing.Factory;
using HoboKing.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using tainicom.Aether.Physics2D.Diagnostics;

namespace HoboKing.Entities
{
    class EntityManager
    {
        readonly static List<GameEntity> _entities = new List<GameEntity>();
        readonly List<GameEntity> _entitiesToAdd = new List<GameEntity>();
        readonly List<GameEntity> _entitiesToRemove = new List<GameEntity>();

        public static IEnumerable<GameEntity> Entities => new ReadOnlyCollection<GameEntity>(_entities);

        public int PlayerCount { get; set; }
        public List<Player> players = new List<Player>();

        public Player mainPlayer;

        public void AddEntity(GameEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Null cannot be added as an entity");
            }
            if (entity.GetType() == typeof(Player))
            {
                Player player = (Player)entity;
                players.Add(player);
                PlayerCount++;
                // Saves the main player in a variable.
                if (player.IsOtherPlayer == false)
                {
                    mainPlayer = player;
                }
            }
            _entitiesToAdd.Add(entity);
        }

        public void RemoveEntity(GameEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Null cannot be removed");
            }

            if (entity.GetType() == typeof(Player))
            {
                players.Remove((Player)entity);
                PlayerCount--;
            }
            _entitiesToRemove.Add(entity);
        }

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

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(var entity in _entities)
            {
                entity.Draw(spriteBatch);
            }
        }

        public void Clear()
        {
            _entitiesToRemove.AddRange(_entities);
        }

        public List<Tile> GetTiles()
        {
            List<Tile> tiles = new List<Tile>();
            foreach(var entity in _entitiesToAdd)
            {
                //if(entity.GetType() == typeof(NormalStandardTile) || entity.GetType() == typeof(IceStandardTile) || entity.GetType() == typeof(SlowStandardTile))
                //{
                //    tiles.Add(entity as Tile);
                //}

                if(entity is Tile)
                {
                    tiles.Add(entity as Tile);
                }
            }

            return tiles;
        }
    }
}
