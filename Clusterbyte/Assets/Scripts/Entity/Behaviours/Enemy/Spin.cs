using UnityEngine;

namespace Entity.Behaviours.Enemy
{
    /// <summary>
    /// Rotate among the Y-axis.
    /// </summary>
    public class Spin : MonoBehaviour
    {
        internal void Update()
        {
            transform.Rotate(Vector3.up, 180 * Time.deltaTime);
        }
    }
}