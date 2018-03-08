using System;

namespace CCore.Senary.Players
{
    [Serializable]
    public class Player
    {
        private PlayerID playerID;

        public PlayerID PlayerID { get { return playerID; } }

        public Player(PlayerID playerID)
        {
            this.playerID = playerID;
        }
    }
}
