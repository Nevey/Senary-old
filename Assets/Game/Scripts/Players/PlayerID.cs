using System;
using UnityEngine;

namespace CCore.Senary.Players
{
    [Serializable]
    public class PlayerID
    {
        public int ID { get; private set; }

        public string Name { get; private set; }

        public Color Color { get; private set; }

        public PlayerID(int id, string name, Color color)
        {
            ID = id;

            Name = name;

            Color = color;
        }
    }
}