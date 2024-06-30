using System.Collections;
using UnityEngine;

namespace Entity.Behaviours.Placeables
{
    /// <summary>
    /// Helper script attached to placeables that locks their kinematic state after a short delay.
    /// </summary>
    public class AutoLockKinematics : MonoBehaviour
    {
        private Rigidbody rb;

        internal void Awake()
        {
            TryGetComponent(out rb);
        }

        internal void Start()
        {
            StartCoroutine(LockKinematic());
        }

        private IEnumerator LockKinematic()
        {
            yield return new WaitForSeconds(0.5f);
            rb.isKinematic = true;
        }
    }
}