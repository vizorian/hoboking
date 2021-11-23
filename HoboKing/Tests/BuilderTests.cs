using HoboKing.Entities;
using HoboKing.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Xunit;

namespace HoboKing.Tests
{
    [ExcludeFromCodeCoverage]
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
