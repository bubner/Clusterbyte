using UnityEngine;

namespace Entity.Behaviours.Enemy
{
    /// <summary>
    /// Represents an enemy that will destroy itself when its health reaches zero.
    /// </summary>
    public class Health : MonoBehaviour
    {
        public float startingHealth = 100f;
        public float health { get; private set; }

        internal void Awake()
        {
            health = startingHealth;
        }

        public void TakeDamage(float amount)
        {
            health -= amount;
            if (health <= 0)
                Destroy(gameObject);
        }
    }
}