using Entity.Behaviours.Enemy;
using UnityEngine;

namespace Entity.Behaviours.Placeables.Turret
{
    /// <summary>
    /// A bullet or shell that is instantiated upon every firing of a turret.
    /// </summary>
    public class Shell : MonoBehaviour
    {
        [SerializeField] private float maxDamage = 34f;
        [SerializeField] private float explosionRadius = 5f;
        [SerializeField] private float maxLifeTime = 2f;

        [SerializeField] private ParticleSystem explosionParticles;
        [SerializeField] private AudioSource fireSound;

        private bool hasActivated;

        internal void Awake()
        {
            if (fireSound.clip.length > maxLifeTime)
                Debug.LogWarning("SFX for shell explosion is longer than the max lifetime of the shell. This may cause unwanted clipping.");
        }

        internal void Start()
        {
            Destroy(gameObject, maxLifeTime);
            fireSound.Play();
        }

        internal void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.CompareTag("Enemy") && !other.gameObject.CompareTag("PlayingField") &&
                !other.gameObject.CompareTag("Terrain"))
                return;

            // We only want to activate the shell once
            if (hasActivated) return;
            hasActivated = true;

            // Make an explosive sphere
            Collider[] colliders = new Collider[30];
            int size = Physics.OverlapSphereNonAlloc(transform.position, explosionRadius, colliders);
            for (int i = 0; i < size; i++)
            {
                if (!colliders[i].gameObject.CompareTag("Enemy"))
                    continue;
                // Calculate damage based on distance from the explosion
                float damage = CalculateDamage(colliders[i].transform.position);
                // Debug.Log("Applying damage to " + hit.gameObject.name + " with " + damage + " damage.");

                if (colliders[i].gameObject.TryGetComponent(out Health health))
                    health.TakeDamage(damage);
            }

            // Unparent particles from the shell
            explosionParticles.transform.parent = null;
            explosionParticles.Play();

            // Remove particles once they are completed
            Destroy(explosionParticles.gameObject, explosionParticles.main.duration);

            // Stop rendering and performing physics on the shell but keep it alive long enough to play the SFX
            // Might be a future task to integrate the SFX into the particle system/as a separate GameObject
            Destroy(GetComponent<Renderer>());
            Destroy(GetComponent<Collider>());
            Destroy(gameObject, fireSound.clip.length - fireSound.time);
        }

        private float CalculateDamage(Vector3 target)
        {
            // Delta between the target and explosion origin
            Vector3 explosionToTarget = target - transform.position;
            float explosionDistance = explosionToTarget.magnitude;
            float relativeDistance = (explosionRadius - explosionDistance) / explosionRadius;
            // Scale based on distance and maximum allowed damage
            return Mathf.Max(0f, relativeDistance * maxDamage);
        }
    }
}