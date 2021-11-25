using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using HoboKing.Entities;
using HoboKing.Factory;
using Xunit;

namespace HoboKing.Tests
{
    [ExcludeFromCodeCoverage]
    public class SectionFactoryTests
    {
        private readonly Creator creator = new MapCreator();
        private readonly EntityManager entityManager = new EntityManager();
        private readonly string Level = "#";
        private readonly int MAP_HEIGHT = 150;

        private readonly int MAP_WIDTH = 64;
        private readonly List<Tile> standardTiles;

        public SectionFactoryTests()
        {
            standardTiles = entityManager.GetTiles();
        }

        [Fact]
        public void GrassSectionTest()
        {
            var sandSection = creator.CreateMapSection(standardTiles, Level, MAP_WIDTH, MAP_HEIGHT, 0, 50);

            Assert.NotNull(sandSection);
        }

        [Fact]
        public void IceSectionTest()
        {
            var iceSection = creator.CreateMapSection(standardTiles, Level, MAP_WIDTH, MAP_HEIGHT, 50, 100);

            Assert.NotNull(iceSection);
        }

        [Fact]
        public void SandSectionTest()
        {
            var grassSection = creator.CreateMapSection(standardTiles, Level, MAP_WIDTH, MAP_HEIGHT, 100, 150);

            Assert.NotNull(grassSection);
        }

        [Fact]
        public void NullSectionTest()
        {
            var nullSection = creator.CreateMapSection(standardTiles, Level, MAP_WIDTH, MAP_HEIGHT, 200, 200);

            Assert.Null(nullSection);
        }
    }
}