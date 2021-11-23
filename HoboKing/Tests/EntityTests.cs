using HoboKing.Entities;
using HoboKing.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using tainicom.Aether.Physics2D.Dynamics;
using Xunit;

namespace HoboKing.Tests
{
    [ExcludeFromCodeCoverage]
    public class EntityTests
    {
        EntityManager entityManager;
        public EntityTests()
        {
           entityManager = new EntityManager();
        }

        [Fact]
        public void AddNullEntity()
        {
            Assert.Throws<ArgumentNullException>(() => entityManager.AddEntity(null));
        }

        [Fact]
        public void RemoveNullEntity()
        {
            Assert.Throws<ArgumentNullException>(() => entityManager.RemoveEntity(null));
        }

        [Fact]
        public void RemovePlayerEntity()
        {
            World world = new World();
            Player player = new Player(null, new Vector2(0, 0), "", false, world);
            entityManager.AddEntity(player);

            entityManager.RemoveEntity(player);

            var players = EntityManager.Entities.Where(p => p is Player);

            Assert.DoesNotContain(player, players);
        }

        [Fact]
        public void EntityUpdateAddPlayer()
        {
            World world = new World();
            Player player = new Player(null, new Vector2(0, 0), "", false, world);
            entityManager.AddEntity(player);
            entityManager.Update(new GameTime());

            //entityManager.RemoveEntity(player);

            var players = EntityManager.Entities.Where(p => p is Player);
            Assert.Contains(player, players);
        }

        [Fact]
        public void EntityUpdateRemovePlayer()
        {
            World world = new World();
            Player player = new Player(null, new Vector2(0, 0), "", false, world);
            entityManager.AddEntity(player);
            entityManager.Update(new GameTime());

            entityManager.RemoveEntity(player);
            entityManager.Update(new GameTime());

            var players = EntityManager.Entities.Where(p => p is Player);
            Assert.DoesNotContain(player, players);
        }

        [Fact]
        public void EntityUpdateAddCritter()
        {
            World world = new World();
            var critter = new CritterBuilder().Build();
            entityManager.AddEntity(critter);
            entityManager.Update(new GameTime());

            var critters = EntityManager.Entities.Where(c => c is Critter);
            Assert.Contains(critter, critters);
        }

    }
}
