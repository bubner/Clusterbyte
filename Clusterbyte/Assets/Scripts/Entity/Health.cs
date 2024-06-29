using Lib;
using UnityEngine;

namespace Entity
{
    public class Health : UIExtensible
    {
        [SerializeField] private float startingHealth = 100f;
        public float health { get; private set; }

        public float GetHealthRatio01()
        {
            return health / startingHealth;
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