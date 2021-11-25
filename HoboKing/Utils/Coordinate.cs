using System.Diagnostics.CodeAnalysis;

namespace HoboKing.Utils
{
    [ExcludeFromCodeCoverage]
    internal class Coordinate
    {
        public Coordinate(string connectionId, float x, float y)
        {
            ConnectionId = connectionId;
            X = x;
            Y = y;
        }

        public string ConnectionId { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
    }
}