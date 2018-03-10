using UnityEngine;

namespace CCore.Senary.Gameplay.Tiles
{
    // TODO: Rename to TileView or TileVisualizer
    public class HQVisualizer : MonoBehaviour
    {
        [SerializeField] private GameObject hqGameObject;

        public void SetHQVisible(bool visible)
        {
            hqGameObject.SetActive(visible);
        }
    }
}