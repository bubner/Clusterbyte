using Entity.Behaviours.Enemy;
using UnityEngine;

namespace Entity.Behaviours.Placeables
{
    /// <summary>
    /// High rate of fire turret.
    /// </summary>
    public class Gunner : MonoBehaviour
    {
        /// <summary>
        /// Launch force for all bullets.
        /// </summary>
        public float launchForce = 12;

        /// <summary>
        /// Firing interval (sec)
        /// </summary>
        public float fireInterval = 0.1f;

        [SerializeField] private Transform head;
        [SerializeField] private Transform fireSpot;
        [SerializeField] private GameObject bulletPrefab;

        private GameObject target;
        private Health targetHealth;
        private float timer;

        internal void Update()
        {
            if (target == null)
            {
                // Lerp up and down in the same way LaserBlaster does it when idle, slight offset for timing aesthetic
                head.rotation = Quaternion.Euler(Mathf.Lerp(-25, 25, Mathf.PingPong(Time.time + 0.4f, 1)), 0, 0);
                transform.rotation = Quaternion.identity;
                return;
            }
            head.LookAt(target.transform);

            if (Time.time < timer + fireInterval)
                return;

            Rigidbody bullet = Instantiate(bulletPrefab, fireSpot.position, fireSpot.rotation).GetComponent<Rigidbody>();
            bullet.velocity = fireSpot.transform.forward * launchForce;
            timer = Time.time;
        }

        internal void OnTriggerStay(Collider other)
        {
            if (!other.gameObject.TryGetComponent(out Health otherHealth))
                return;
            if (target != null && otherHealth.health <= targetHealth.health)
                return;
            // No need to release the target if we don't find another one, as this weapon is designed to be long range
            // if required unlike the Turret
            target = other.gameObject;
            targetHealth = otherHealth;
        }
    }
}