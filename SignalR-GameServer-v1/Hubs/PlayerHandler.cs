using System;
using System.Collections.Generic;

namespace SignalR_GameServer.Hubs
{
    public class PlayerHandler
    {
        private static readonly object lockObj = new object();
        private static PlayerHandler playerHandler;
        public List<String> ConnectedIds;

        private PlayerHandler()
        {
            ConnectedIds = new List<String>();
        }

        public static PlayerHandler getInstance()
        {
            if(playerHandler == null)
            {
                lock (lockObj)
                {
                    if(playerHandler == null)
                    {
                        playerHandler = new PlayerHandler();
                    }
                }
            }
            return playerHandler;
        }
    }
}
