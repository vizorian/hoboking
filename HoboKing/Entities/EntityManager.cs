﻿using Microsoft.Xna.Framework;
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
        public void AddEntity(IGameEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Null cannot be added as an entity");
            }
            _entitiesToAdd.Add(entity);
        }

        public void RemoveEntity(IGameEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), "Null cannot be removed");
            }
            _entitiesToRemove.Add(entity);
        }

        public void Update(GameTime gameTime)
        {
            foreach(IGameEntity entity in _entities)
            {
                entity.Update(gameTime);
            }
            foreach(IGameEntity entity in _entitiesToAdd)
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

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach(IGameEntity entity in _entities)
            {
                entity.Draw(spriteBatch, gameTime);
            }
        }

        public void Clear()
        {
            _entitiesToRemove.AddRange(_entities);
        }
    }
}
