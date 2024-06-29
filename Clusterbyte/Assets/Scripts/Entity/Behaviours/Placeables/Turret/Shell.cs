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
            // TODO: Use nonAlloc
            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
            foreach (Collider hit in colliders)
            {
                if (!hit.gameObject.CompareTag("Enemy"))
                    continue;
                // Calculate damage based on distance from the explosion
                float damage = CalculateDamage(hit.transform.position);
                Debug.Log("Applying damage to " + hit.gameObject.name + " with " + damage + " damage.");

                if (hit.gameObject.TryGetComponent(out Health health))
                    health.TakeDamage(damage);
            }

            // Unparent particles from the shell
            explosionParticles.transform.parent = null;
            explosionParticles.Play();

            // Remove particles once they are completed
            Destroy(explosionParticles.gameObject, explosionParticles.main.duration);

            // // Stop rendering and performing physics on the shell but keep it alive long enough to play the SFX
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