using HoboKing.Entities.Factory;
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
        readonly static List<IGameEntity> _entities = new List<IGameEntity>();
        readonly List<IGameEntity> _entitiesToAdd = new List<IGameEntity>();
        readonly List<IGameEntity> _entitiesToRemove = new List<IGameEntity>();

        public static IEnumerable<IGameEntity> Entities => new ReadOnlyCollection<IGameEntity>(_entities);

        public int PlayerCount { get; set; }
        public List<Player> players = new List<Player>();

        private Player mainPlayer;

        public void AddEntity(IGameEntity entity)
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

        public void RemoveEntity(IGameEntity entity)
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
            foreach (IGameEntity entity in _entities)
            {
                if (entity is Player && (entity as Player).IsOtherPlayer == false)
                {
                    entity.Update(gameTime);
                }
            }
            foreach (IGameEntity entity in _entitiesToAdd)
            {
                _entities.Add(entity);
            }
            _entitiesToAdd.Clear();
            foreach (IGameEntity entity in _entitiesToRemove)
            {

                _entities.Remove(entity);

            }
            _entitiesToRemove.Clear();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(IGameEntity entity in _entities)
            {
                entity.Draw(spriteBatch);
            }
        }

        public void DrawDebug(DebugView debugView, GraphicsDevice graphics, ContentManager content)
        {
            foreach (IGameEntity entity in _entities)
            {
                entity.Sprite.DrawDebug(debugView, graphics, content);
            }
        }

        public void Clear()
        {
            _entitiesToRemove.AddRange(_entities);
        }

        public List<Tile> GetStandardTiles()
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
