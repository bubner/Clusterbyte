using UnityEngine;

namespace Entity.Behaviours.Enemy
{
    public class Health : MonoBehaviour
    {
        public float startingHealth = 100f;
        public float health { get; private set; }

        internal void Awake()
        {
            ResetHealth();
        }

        public void ResetHealth()
        {
            health = startingHealth;
        }

        public void TakeDamage(float damage)
        {
            if (health - damage >= 0)
                health -= damage;
        }

        public bool TryTakeDamage(float damage)
        {
            if (health - damage >= 0)
                health -= damage;
            return health <= 0;
        }

        public bool IsDead()
        {
            return health <= 0;
        }
    }
}