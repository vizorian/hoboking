using HoboKing.Entities;
using HoboKing.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace HoboKing.Tests
{
    public class BuilderTests
    {
        public BuilderTests()
        {

        }

        // CRITTER

        [Fact]
        public void CritterBuildTest()
        {
            var critter = new CritterBuilder().Build();
            Assert.NotNull(critter);
        }

        [Fact]
        public void CritterAddMovementTest()
        {
            var addedMovement = new CritterBuilder().AddMovement();
            Assert.IsType<CritterBuilder>(addedMovement);
        }

        [Theory]
        [InlineData("Hello", 1)]
        [InlineData("", -1)]
        public void CritterAddSpeechTest(string speech, int triggerSize)
        {
            var addedSpeech = new CritterBuilder().AddSpeech(speech, triggerSize);
            Assert.IsType<CritterBuilder>(addedSpeech);
        }

        [Fact]
        public void DeepCopyCritterTest()
        {
            var critter = new Critter();
            var copy = critter.DeepCopy();
            Assert.NotEqual(critter.GetHashCode(), copy.GetHashCode());
        }

        [Fact]
        public void CritterUpdateTest()
        {
            var game = new HoboKingGame();
            game.RunOneFrame();
            var critter = new CritterBuilder().AddTexture(ContentLoader.GrassLeft,
                new Vector2(0, 0)).AddMovement().Build() as Critter;
            var oldPosition = critter.Position.X;
            critter.Update(new GameTime());
            var newPosition = critter.Position.X;
            Assert.NotEqual(oldPosition, newPosition);
        }

        // OBJECT

        [Fact]
        public void ObjectBuildTest()
        {
            var obj = new ObjectBuilder().Build();
            Assert.NotNull(obj);
        }

        [Fact]
        public void ObjectAddMovementTest()
        {
            var addedMovement = new ObjectBuilder().AddMovement();
            Assert.IsType<ObjectBuilder>(addedMovement);
        }

        [Theory]
        [InlineData("Hello", 1)]
        [InlineData("", -1)]
        public void ObjectAddSpeechTest(string speech, int triggerSize)
        {
            var addedSpeech = new ObjectBuilder().AddSpeech(speech, triggerSize);
            Assert.IsType<ObjectBuilder>(addedSpeech);
        }

        [Fact]
        public void DeepCopyObjectTest()
        {
            var obj = new Entities.Object();
            var copy = obj.DeepCopy();
            Assert.NotEqual(obj.GetHashCode(), copy.GetHashCode());
        }
    }
}
