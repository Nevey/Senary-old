namespace CCore.Senary.Players
{
    public class PlayerID
    {
        public int ID { get; private set; }

        public string Name { get; private set; }

        public PlayerID(int id, string name)
        {
            ID = id;

            Name = name;
        }
    }
}