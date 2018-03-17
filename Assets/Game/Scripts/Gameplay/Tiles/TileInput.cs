using System;
using CCore.Senary.Input;
using UnityEngine;

namespace CCore.Senary.Gameplay.Tiles
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(TileData))]
    public class TileInput : MonoBehaviour
    {
        private new Collider collider;

        private TileData tileData;

        public event Action TileTappedEvent;

        private void Awake()
        {
            collider = GetComponent<Collider>();

            tileData = GetComponent<TileData>();
        }

        private void OnEnable()
        {
            PlayerInput.Instance.TapEvent += OnTap;
        }

        private void OnDisable()
        {
            // During editor time this is already null
            if (PlayerInput.Instance != null)
            {
                PlayerInput.Instance.TapEvent -= OnTap;
            }
        }

        private void OnTap(Vector2 position)
        {
            RaycastHit hit;

            Ray ray = Camera.main.ScreenPointToRay(position);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider == collider)
                {
                    Log("Tapped on tile {0}", tileData.Tile.Name);

                    if (TileTappedEvent != null)
                    {
                        TileTappedEvent();
                    }
                }
            }
        }
    }
}