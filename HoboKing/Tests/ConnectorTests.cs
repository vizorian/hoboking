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
using System.Threading.Tasks;
using Xunit;

namespace HoboKing.Tests
{
    [ExcludeFromCodeCoverage]
    public class ConnectorTests
    {
        public ConnectorTests()
        {

        }

        [Theory]
        [InlineData(10, 10)]
        [InlineData(10, 5)]
        [InlineData(10, 0.05)]
        public async void SendData(int totalTime, int elapsedTime)
        {
            ConnectorComponent connector = new ConnectorComponent();
            connector.CreateListeners();
            await connector.Connect();

            var ex = await Record.ExceptionAsync(() => connector.SendData(new GameTime(new TimeSpan(0, 0, totalTime), new TimeSpan(0, 0, elapsedTime)), new Vector2(20, 20)));

            Assert.Null(ex);
        }

        [Fact]
        public async void Connect()
        {
            ConnectorComponent connector = new ConnectorComponent();
            connector.CreateListeners();

            var ex = await Record.ExceptionAsync(() => connector.Connect());
            Assert.Null(ex);
        }

        [Fact]
        public async void Disconnect()
        {
            ConnectorComponent connector = new ConnectorComponent();
            connector.CreateListeners();

            await connector.Connect();

            var ex = await Record.ExceptionAsync(() => connector.Disconnect());
            Assert.Null(ex);
        }

        [Fact]
        public async void ConnectException()
        {
            ConnectorComponent connector = new ConnectorComponent();
            await Assert.ThrowsAsync<Exception>(() => connector.Connect());
        }

        [Fact]
        public async void DisconnectException()
        {
            ConnectorComponent connector = new ConnectorComponent();
            await Assert.ThrowsAsync<Exception>(() => connector.Disconnect());
        }

        [Fact]
        public async void GetConnectionId()
        {
            ConnectorComponent connector = new ConnectorComponent();
            connector.CreateListeners();
            await connector.Connect();

            Assert.NotNull(connector.GetConnectionId());
        }
    }
}
