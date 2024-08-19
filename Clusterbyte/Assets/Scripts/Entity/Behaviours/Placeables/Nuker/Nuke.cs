using UnityEngine;

namespace Entity.Behaviours.Placeables.Nuker
{
    public class Nuke : MonoBehaviour
    {
        [SerializeField] private GameObject explosionPrefab;

        // Arg-less, we don't care about what we hit
        internal void OnCollisionEnter()
        {
            // Nuke should disappear instantly and explosion will be handled by the particles
            // TODO: destruction
            Destroy(gameObject);
            Instantiate(explosionPrefab);
        }
    }
}