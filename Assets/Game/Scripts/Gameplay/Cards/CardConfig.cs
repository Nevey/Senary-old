using UnityEngine;

namespace Game.Scripts.Gameplay.Cards
{
    [CreateAssetMenu(fileName = "CardConfig", menuName = "CCore/CardConfig")]
    public class CardConfig : ScriptableObject
    {
        [Header("Default Card Movement")]
        [SerializeField] private float moveDuration = 0.1f;

        [SerializeField] private float maxMoveSpeed = 100f;

        [Header("Card Movement When Dragged")]
        [SerializeField] private float dragMoveDuration = 0.1f;

        [SerializeField] private float dragMaxMoveSpeed = 100f;

        [SerializeField] private float dragCardHeigt = 15f;

        public float MoveDuration
        {
            get { return moveDuration; }
        }

        public float MaxMoveSpeed
        {
            get { return maxMoveSpeed; }
        }

        public float DragMoveDuration
        {
            get { return dragMoveDuration; }
        }

        public float DragMaxMoveSpeed
        {
            get { return dragMaxMoveSpeed; }
        }

        public float DragCardHeigt
        {
            get { return dragCardHeigt; }
        }
    }
}