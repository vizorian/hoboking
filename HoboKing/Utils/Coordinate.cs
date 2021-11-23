using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace HoboKing.Entities
{
    [ExcludeFromCodeCoverage]
    class Coordinate
    {
        public string ConnectionID { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public Coordinate(string connectionID, float x, float y)
        {
            ConnectionID = connectionID;
            X = x;
            Y = y;
        }
    }
}
