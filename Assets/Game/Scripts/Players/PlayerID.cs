using System;
using UnityEngine;

namespace CCore.Senary.Players
{
    [Serializable]
    public class PlayerID
    {
        [SerializeField]
        private int id;

        [SerializeField]
        private string name = "";

        [SerializeField]
        private Color color;

        public int ID { get { return id; } }

        public string Name { get { return name; } }

        public Color Color { get { return color; } }

        /// <summary>
        /// Returns a dummy player, use this to un-own a tile
        /// </summary>
        /// <returns></returns>
        public static PlayerID Dummy
        {
            get
            {
                return new PlayerID(-1, "None", Color.white);
            }
        }

        public PlayerID(int id, string name, Color color)
        {
            this.id = id;

            this.name = name;

            this.color = color;
        }
    }
}
