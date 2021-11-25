using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using HoboKing.Builder;
using HoboKing.Entities;
using Microsoft.Xna.Framework;
using tainicom.Aether.Physics2D.Dynamics;
using Xunit;

namespace HoboKing.Tests
{
    [ExcludeFromCodeCoverage]
    public class EntityTests
    {
        private readonly EntityManager entityManager;

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
            var world = new World();
            var player = new Player(null, new Vector2(0, 0), "", false, world);
            entityManager.AddEntity(player);

            entityManager.RemoveEntity(player);

            var players = EntityManager.EntitiesNum.Where(p => p is Player);

            Assert.DoesNotContain(player, players);
        }

        [Fact]
        public void EntityUpdateAddPlayer()
        {
            var world = new World();
            var player = new Player(null, new Vector2(0, 0), "", false, world);
            entityManager.AddEntity(player);
            entityManager.Update(new GameTime());

            //entityManager.RemoveEntity(player);

            var players = EntityManager.EntitiesNum.Where(p => p is Player);
            Assert.Contains(player, players);
        }

        [Fact]
        public void EntityUpdateRemovePlayer()
        {
            var world = new World();
            var player = new Player(null, new Vector2(0, 0), "", false, world);
            entityManager.AddEntity(player);
            entityManager.Update(new GameTime());

            entityManager.RemoveEntity(player);
            entityManager.Update(new GameTime());

            var players = EntityManager.EntitiesNum.Where(p => p is Player);
            Assert.DoesNotContain(player, players);
        }

        [Fact]
        public void EntityUpdateAddCritter()
        {
            var critter = new CritterBuilder().Build();
            entityManager.AddEntity(critter);
            entityManager.Update(new GameTime());

            var critters = EntityManager.EntitiesNum.Where(c => c is Critter);
            Assert.Contains(critter, critters);
        }
    }
}