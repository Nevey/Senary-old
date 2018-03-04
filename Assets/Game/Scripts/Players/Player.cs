namespace CCore.Senary.Players
{
    public class Player
    {
        public PlayerID PlayerID { get; private set; }

        public Player(PlayerID playerID)
        {
            PlayerID = playerID;
        }
    }
}