using System;
using System.Collections.Generic;
using System.Text;

namespace HoboKing.Entities
{
    class PlayerManager
    {
        public int PlayerCount { get; set; }
        EntityManager EntityManager;
        public List<OtherPlayer> playerIDs;

        public PlayerManager(EntityManager entityManager)
        {
            PlayerCount = 0;
            EntityManager = entityManager;
            playerIDs = new List<OtherPlayer>();
        }

        public void CreatePlayer(OtherPlayer p)
        {
            EntityManager.AddEntity(p);
            playerIDs.Add(p);
            PlayerCount++;
        }

        public void DeletePlayer(OtherPlayer p)
        {
            EntityManager.RemoveEntity(p);
            playerIDs.Remove(p);
            PlayerCount--;
        }
    }
}
