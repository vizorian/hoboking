using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HoboKing.Entities
{
    class PlayerManager
    {
        public int PlayerCount { get; set; }
        EntityManager EntityManager;
        public List<OtherPlayer> players;

        public PlayerManager(EntityManager entityManager)
        {
            PlayerCount = 0;
            EntityManager = entityManager;
            players = new List<OtherPlayer>();
        }

        public void CreateOtherPlayer(OtherPlayer p)
        {
            EntityManager.AddEntity(p);
            players.Add(p);
            PlayerCount++;
        }

        public void RemoveOtherPlayer(OtherPlayer p)
        {
            EntityManager.RemoveEntity(p);
            players.Remove(p);
            PlayerCount--;
        }

        public Player CreatePlayer(Texture2D texture, Vector2 position, SoundEffect jumpSound, Map map)
        {
            return new Player(texture, position, jumpSound, map);
        }
    }
}
