using UnityEngine;

namespace Entity.Behaviours.Placeables.Turret
{
    /// <summary>
    /// Auto-aiming turret that fires shells at enemies.
    /// </summary>
    public class Turret : MonoBehaviour
    {
        public float fireTime = 1f;
        public float launchForce = 10f;
        public float radius = 3f;

        [SerializeField] private Transform fireLocation;
        [SerializeField] private Transform head;
        [SerializeField] private Rigidbody shellPrefab;

        private GameObject target;
        private float timer;

        internal void Update()
        {
            if (!target)
            {
                // If there's nothing to shoot, rotate in place to simulate looking for enemies
                head.rotation = Quaternion.Euler(0, head.rotation.eulerAngles.y, 0);
                head.Rotate(Vector3.up, 30 * Time.deltaTime);
                return;
            }

            // Check if the turret is too far away from the enemy
            if (Vector3.Distance(transform.position, target.transform.position) > radius)
            {
                target = null;
                return;
            }

            head.LookAt(target.transform);

            // Only fire once every fireTime seconds
            timer -= Time.deltaTime;
            if (timer > 0)
                return;
            timer = fireTime;

            // Fire a shell
            Rigidbody newShell = Instantiate(shellPrefab, fireLocation.position, fireLocation.rotation);
            newShell.velocity = launchForce * fireLocation.forward;
        }

        internal void OnTriggerStay(Collider other)
        {
            // Only lock onto one enemy at a time
            if (!other.CompareTag("Enemy") || (target != null && other.gameObject != target))
                return;
            target = other.gameObject;
        }
    }
}