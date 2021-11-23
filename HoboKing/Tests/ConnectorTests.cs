using HoboKing.Entities;
using HoboKing.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using Xunit;

namespace HoboKing.Tests
{
    [ExcludeFromCodeCoverage]
    public class ConnectorTests
    {
        public ConnectorTests()
        {

        }

        [Fact]
        public async void SendData()
        {
            ConnectorComponent connector = new ConnectorComponent();
            connector.CreateListeners();
            bool worked = true;
            await connector.Connect();
            try
            {
                connector.SendData(new GameTime(), new Vector2(20, 20));
            }
            catch (Exception)
            {
                worked = false;
            }

            connector.Disconnect();
            Assert.True(worked);
        }
    }
}
