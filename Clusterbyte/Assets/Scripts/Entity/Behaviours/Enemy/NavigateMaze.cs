using UnityEngine;
using UnityEngine.AI;

namespace Entity.Behaviours.Enemy
{
    /// <summary>
    /// A generic enemy that tries to move towards the end of the path.
    /// </summary>
    public class NavigateMaze : MonoBehaviour
    {
        /// <summary>
        /// Whether this enemy will subtract a life from the player when it reaches the end of the path.
        /// </summary>
        public bool isHarmful;

        private NavMeshAgent agent;

        internal void Awake()
        {
            TryGetComponent(out agent);
        }

        internal void Update()
        {
            // Move the agent towards the end of the path as defined by the GameManager
            agent.destination = GameManager.instance.entityTarget.transform.position;
            // If the agent has reached the end of the path, the player loses a life
            if (!agent.hasPath || agent.remainingDistance >= Clusterbyte.END_OF_PATH_TRIGGER_BOX_THRESHOLD)
                return;
            if (isHarmful)
                GameManager.instance.playerStats.LoseLife();
            Destroy(gameObject);
        }
    }
}