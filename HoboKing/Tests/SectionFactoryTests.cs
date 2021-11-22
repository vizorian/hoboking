using HoboKing.Factory;
using HoboKing.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Diagnostics.CodeAnalysis;

namespace HoboKing.Tests
{
    [ExcludeFromCodeCoverage]
    public class SectionFactoryTests
    {
        
        int MAP_WIDTH = 64;
        int MAP_HEIGHT = 150;
        string Level = "#";
        Creator creator = new MapCreator();
        EntityManager entityManager = new EntityManager();
        List<Tile> standardTiles;

        public SectionFactoryTests()
        {
            standardTiles = entityManager.GetTiles();
        }

        [Fact]
        public void GrassSectionTest()
        { 
            Section sandSection = creator.CreateMapSection(standardTiles, Level, MAP_WIDTH, MAP_HEIGHT, 0, 50);

            Assert.NotNull(sandSection);
        }

        [Fact]
        public void IceSectionTest()
        {
            Section iceSection = creator.CreateMapSection(standardTiles, Level, MAP_WIDTH, MAP_HEIGHT, 50, 100);

            Assert.NotNull(iceSection);
        }

        [Fact]
        public void SandSectionTest()
        {
            Section grassSection = creator.CreateMapSection(standardTiles, Level, MAP_WIDTH, MAP_HEIGHT, 100, 150);

            Assert.NotNull(grassSection);
        }

        [Fact]
        public void NullSectionTest()
        {
            Section nullSection = creator.CreateMapSection(standardTiles, Level, MAP_WIDTH, MAP_HEIGHT, 200, 200);

            Assert.Null(nullSection);
        }
    }
}
