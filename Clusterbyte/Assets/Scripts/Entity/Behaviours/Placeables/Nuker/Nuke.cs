using System.Collections.Generic;
using System.Linq;
using Entity.Behaviours.Enemy;
using Entity.Factory.Types;
using UnityEngine;

namespace Entity.Behaviours.Placeables.Nuker
{
    /// <summary>
    /// Destroys everything within radius on impact.
    /// </summary>
    public class Nuke : MonoBehaviour
    {
        /// <summary>
        /// Radius of explosion to destroy everything within.
        /// </summary>
        public float nukeRadius = 6;

        [SerializeField] private GameObject explosionPrefab;
        private Vector3 velocity;

        internal void Update()
        {
            velocity += Physics.gravity * Time.deltaTime;
            transform.position += velocity * Time.deltaTime;
        }

        internal void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Nuke") || transform.position.y > 3)
            {
                // Ignore other nukes or triggers that are too high
                return;
            }

            // Apply damage (immediate destruction) to everything in radius. Need to use a high max allocation for this.
            Collider[] hits = new Collider[200];
            List<GameObject> playerPlacedAffected = new();
            int size = Physics.OverlapSphereNonAlloc(transform.position, nukeRadius, hits);
            for (int i = 0; i < size; i++)
            {
                GameObject obj = hits[i].gameObject;
                if (obj.layer == 2)
                {
                    // Will handle this later. Need to do some special calculations based on terrain
                    playerPlacedAffected.Add(obj);
                    continue;
                }
                if (obj.CompareTag("PlayingField"))
                {
                    // Should ignore the playing field, that is our only thing we shouldn't remove
                    continue;
                }
                // Destroy everything else, including terrain and user-placed items
                if (obj.TryGetComponent(out Health health))
                    health.TakeDamage(Mathf.Infinity);
                obj.SetActive(false);
            }

            foreach (ShopDeployable deployable in GameManager.instance.deployed.Where(deployable => playerPlacedAffected.Contains(deployable.instance)))
            {
                if (!Physics.Raycast(deployable.instance.transform.position, Vector3.down, out RaycastHit hit))
                    continue;

                if (!hit.collider.gameObject.CompareTag("Terrain"))
                {
                    deployable.instance.SetActive(false);
                }
            }

            // Nuke should disappear instantly and explosion effects will be handled by the particles
            gameObject.SetActive(false);
            Instantiate(explosionPrefab);
        }
    }
}