using Entity.Behaviours.Enemy;
using UnityEngine;

namespace Entity.Behaviours.Placeables.Nuker
{
    public class Nuke : MonoBehaviour
    {
        /// <summary>
        /// Radius of explosion to destroy everything within.
        /// </summary>
        public float nukeRadius = 6;

        [SerializeField] private GameObject explosionPrefab;

        // Arg-less, we don't care about what we hit
        internal void OnCollisionEnter()
        {
            // Apply damage (immediate destruction) to everything in radius. Need to use a high max allocation for this.
            Collider[] hits = new Collider[200];
            int size = Physics.OverlapSphereNonAlloc(transform.position, nukeRadius, hits);
            for (int i = 0; i < size; i++)
            {
                GameObject obj = hits[i].gameObject;
                if (obj.CompareTag("PlayingField"))
                {
                    // Should ignore the playing field, that is our only thing we shouldn't remove
                    continue;
                }
                // Destroy everything else, including terrain and user-placed items
                if (obj.TryGetComponent(out Health health))
                    health.TakeDamage(Mathf.Infinity);
                Destroy(obj);
            }

            // Nuke should disappear instantly and explosion effects will be handled by the particles
            Destroy(gameObject);
            Instantiate(explosionPrefab);
        }
    }
}