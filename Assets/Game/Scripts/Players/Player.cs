using System;

namespace CCore.Senary.Players
{
    [Serializable]
    public class Player
    {
        public PlayerID PlayerID { get; private set; }

        public Player(PlayerID playerID)
        {
            PlayerID = playerID;
        }
    }
}