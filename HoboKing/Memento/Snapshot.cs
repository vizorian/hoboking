using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using HoboKing.Entities;
using tainicom.Aether.Physics2D.Dynamics;

namespace HoboKing.Memento
{
    class Snapshot
    {
        private Vector2 position;
        private DateTime dateTime;
        public Snapshot(Vector2 position)
        {
            this.position = position;
            dateTime = DateTime.Now;
        }

        public Vector2 GetPosition()
        {
            return position;
        }

        public DateTime GetDateTime()
        {
            return dateTime;
        }
    }
}
