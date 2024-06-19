using UnityEngine;

namespace Entity.Placeables
{
    public class Turret : MonoBehaviour
    {
        [SerializeField] private Transform head;
        private GameObject target;
        private bool targetLocked;

        internal void Update()
        {
            if (!targetLocked) return;
            head.LookAt(target.transform);
        }

        internal void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Enemy")) return;
            target = other.gameObject;
            targetLocked = true;
        }

        internal void OnTriggerExit(Collider other)
        {
            // Latch onto the first enemy that enters the trigger
            if (!other.CompareTag("Enemy") || other.gameObject != target) return;
            target = null;
            targetLocked = false;
        }
    }
}