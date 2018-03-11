using UnityEngine;

namespace CCore.Senary.CPhysics
{
    [RequireComponent(typeof(Rigidbody))]
    public class InitialVelocity : MonoBehaviour
    {
        [Header("This component sets a random velocity and angular velocity on Awake")]

        [Space]

        [SerializeField] private Vector3 minInitialVelocity;

        [SerializeField] private Vector3 maxInitialVelocity;

        [SerializeField] private Vector3 minInitialAngularVelocity;

        [SerializeField] private Vector3 maxInitialAngularVelocity;

        private new Rigidbody rigidbody;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();

            Vector3 initialVelocity = new Vector3(
                Random.Range(minInitialVelocity.x, maxInitialVelocity.x),
                Random.Range(minInitialVelocity.y, maxInitialVelocity.y),
                Random.Range(minInitialVelocity.z, maxInitialVelocity.z)
            );

            Vector3 initialAngularVelocity = new Vector3(
                Random.Range(minInitialAngularVelocity.x, maxInitialAngularVelocity.x),
                Random.Range(minInitialAngularVelocity.y, maxInitialAngularVelocity.y),
                Random.Range(minInitialAngularVelocity.z, maxInitialAngularVelocity.z)
            );

            rigidbody.velocity = initialVelocity;

            rigidbody.angularVelocity = initialAngularVelocity;
        }
    }
}