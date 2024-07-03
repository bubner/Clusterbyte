using UnityEngine;

namespace Lib
{
    /// <summary>
    /// Automatically destroys the GameObject after a set amount of time.
    /// </summary>
    public class AutoDestroy : MonoBehaviour
    {
        [SerializeField] private float inSeconds = 1f;

        internal void Start()
        {
            Destroy(gameObject, inSeconds);
        }
    }
}