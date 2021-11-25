using System;
using System.Diagnostics.CodeAnalysis;
using HoboKing.Components;
using Microsoft.Xna.Framework;
using Xunit;

namespace HoboKing.Tests
{
    [ExcludeFromCodeCoverage]
    public class ConnectorTests
    {
        [Theory]
        [InlineData(10, 10)]
        [InlineData(10, 5)]
        [InlineData(10, 0.05)]
        public async void SendData(int totalTime, int elapsedTime)
        {
            var connector = new ConnectorComponent();
            connector.CreateListeners();
            await connector.Connect();

            var ex = await Record.ExceptionAsync(() =>
                connector.SendData(new GameTime(new TimeSpan(0, 0, totalTime), new TimeSpan(0, 0, elapsedTime)),
                    new Vector2(20, 20)));

            Assert.Null(ex);
        }

        [Fact]
        public async void Connect()
        {
            var connector = new ConnectorComponent();
            connector.CreateListeners();

            var ex = await Record.ExceptionAsync(() => connector.Connect());
            Assert.Null(ex);
        }

        [Fact]
        public async void Disconnect()
        {
            var connector = new ConnectorComponent();
            connector.CreateListeners();

            await connector.Connect();

            var ex = await Record.ExceptionAsync(() => connector.Disconnect());
            Assert.Null(ex);
        }

        [Fact]
        public async void ConnectException()
        {
            var connector = new ConnectorComponent();
            await Assert.ThrowsAsync<Exception>(() => connector.Connect());
        }

        [Fact]
        public async void DisconnectException()
        {
            var connector = new ConnectorComponent();
            await Assert.ThrowsAsync<Exception>(() => connector.Disconnect());
        }

        [Fact]
        public async void GetConnectionId()
        {
            var connector = new ConnectorComponent();
            connector.CreateListeners();
            await connector.Connect();

            Assert.NotNull(connector.GetConnectionId());
        }
    }
}