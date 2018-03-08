using System;
using UnityEngine;

namespace CCore.Senary.Players
{
    [Serializable]
    public class PlayerID
    {
        private int id;

        private string name;

        private Color color;

        public int ID { get { return id; } }

        public string Name { get { return name; } }

        public Color Color { get { return color; } }

        public PlayerID(int id, string name, Color color)
        {
            this.id = id;

            this.name = name;

            this.color = color;
        }
    }
}
