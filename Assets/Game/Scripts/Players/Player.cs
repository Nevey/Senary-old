using System;
using UnityEngine;

namespace CCore.Senary.Players
{
    [Serializable]
    public class Player
    {
        [SerializeField]
        private PlayerID playerID;

        public PlayerID PlayerID { get { return playerID; } }

        public static Player Dummy
        {
            get
            {
                return new Player(PlayerID.Dummy);
            }
        }

        public Player(PlayerID playerID)
        {
            this.playerID = playerID;
        }
    }
}
