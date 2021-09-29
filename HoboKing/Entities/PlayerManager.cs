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

        public void CreatePlayer(OtherPlayer p)
        {
            EntityManager.AddEntity(p);
            players.Add(p);
            PlayerCount++;
        }

        public void DeletePlayer(OtherPlayer p)
        {
            EntityManager.RemoveEntity(p);
            players.Remove(p);
            PlayerCount--;
        }
    }
}
