using UnityEngine;

namespace Game.Scripts.Gameplay.Cards
{
    [CreateAssetMenu(fileName = "CardConfig", menuName = "CCore/CardConfig")]
    public class CardConfig : ScriptableObject
    {
        [Header("Default Card Movement")]
        [SerializeField] private float moveDuration = 0.1f;

        [SerializeField] private float maxMoveSpeed = 100f;

        [Header("Card Movement When Held")]
        [SerializeField] private float holdMoveDuration = 0.1f;

        [SerializeField] private float holdMaxMoveSpeed = 100f;

        public float MoveDuration
        {
            get { return moveDuration; }
        }

        public float MaxMoveSpeed
        {
            get { return maxMoveSpeed; }
        }

        public float HoldMoveDuration
        {
            get { return holdMoveDuration; }
        }

        public float HoldMaxMoveSpeed
        {
            get { return holdMaxMoveSpeed; }
        }
    }
}