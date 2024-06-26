using UnityEngine;

namespace Entity.Placeables.Turret
{
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
                head.rotation = Quaternion.Euler(0, head.rotation.eulerAngles.y, 0);
                head.Rotate(Vector3.up, 30 * Time.deltaTime);
                return;
            }

            // Check if the turret is too far away
            if (Vector3.Distance(transform.position, target.transform.position) > radius)
            {
                target = null;
                return;
            }

            head.LookAt(target.transform);

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
            if (!other.CompareTag("Enemy") || (target != null && other.gameObject != target))
                return;
            target = other.gameObject;
        }
    }
}