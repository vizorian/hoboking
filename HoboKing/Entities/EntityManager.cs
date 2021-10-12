using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace HoboKing.Entities
{
    class EntityManager
    {
        readonly List<IGameEntity> _entities = new List<IGameEntity>();
        readonly List<IGameEntity> _entitiesToAdd = new List<IGameEntity>();
        readonly List<IGameEntity> _entitiesToRemove = new List<IGameEntity>();

        public IEnumerable<IGameEntity> Entities => new ReadOnlyCollection<IGameEntity>(_entities);

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
                if (entity is Tile)
                {
                    mainPlayer.CheckCollision(entity);
                }
                else
                {
                    entity.Update(gameTime);
                }
                //if (!(entity is Tile))
                //{
                //    entity.Update(gameTime);
                //}
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

        public void SetShowBoundingBox(bool show)
        {
            foreach(var entity in _entities)
            {
                entity.Sprite.SetShowRectangle(show);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(IGameEntity entity in _entities)
            {
                entity.Draw(spriteBatch);
            }
        }

        public void Clear()
        {
            _entitiesToRemove.AddRange(_entities);
        }

    }
}
