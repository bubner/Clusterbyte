using System;
using UnityEngine;

namespace Entity.Behaviours.Placeables.LaserBlaster
{
    /// <summary>
    /// Auto-aiming blaster that continuously does damage to an enemy.
    /// </summary>
    public class LaserBlaster : MonoBehaviour
    {
        [SerializeField] private Transform head;
        [SerializeField] private Transform fire;

        private LineRenderer laserRenderer;
        private GameObject target;

        internal void Awake()
        {
            laserRenderer = GetComponentInChildren<LineRenderer>();
        }

        internal void Update()
        {
            if (!target)
            {
                laserRenderer.enabled = false;
                return;
            }

            // Lerp to the enemy orientation, with the target being interpreted as slightly lower to let the laser hit
            Vector3 correctedTarget = target.transform.position + Vector3.down * 0.5f;
            head.rotation = Quaternion.Lerp(head.rotation, Quaternion.LookRotation(correctedTarget - head.position), Time.deltaTime * 5);

            // Draw the laser straight forward to pierce through everything until we hit something like a wall
            // Max length of 8 units
            Vector3 laserEndPosition = fire.position + fire.forward * 8;
            if (Physics.Raycast(fire.position, fire.forward, out RaycastHit hit, 8))
            {
                // TODO: fix
               if (!hit.collider.CompareTag("Enemy"))
               {
                   laserEndPosition = hit.point;
               }
            }
            laserRenderer.SetPosition(0, fire.position);
            laserRenderer.SetPosition(1, laserEndPosition);
            laserRenderer.enabled = true;
        }

        internal void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Enemy") && target == null)
                target = other.gameObject;
        }

        internal void OnTriggerExit(Collider other)
        {
            if (other.gameObject == target)
            {
                target = null;
            }
        }
    }
}