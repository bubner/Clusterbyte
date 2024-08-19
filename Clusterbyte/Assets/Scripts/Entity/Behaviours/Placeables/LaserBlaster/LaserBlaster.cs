using System;
using Entity.Behaviours.Enemy;
using UnityEngine;

namespace Entity.Behaviours.Placeables.LaserBlaster
{
    /// <summary>
    /// Auto-aiming blaster that continuously does damage to an enemy.
    /// </summary>
    public class LaserBlaster : MonoBehaviour
    {
        public float damage = 5;

        [SerializeField] private AudioSource laserSound;
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
                // Lerp left and right for an idle animation. We can rely on a consistent incrementation of a variable
                // through Time.time
                head.rotation = Quaternion.Euler(0, Mathf.Lerp(0, 60, Mathf.PingPong(Time.time, 1)), 0);
                transform.rotation = Quaternion.identity;

                laserSound.Stop();
                laserRenderer.enabled = false;
                return;
            }

            // Lerp to the enemy orientation, with the target being interpreted as slightly lower to let the laser hit
            Vector3 correctedTarget = target.transform.position + Vector3.down * 0.5f;
            head.rotation = Quaternion.Lerp(head.rotation, Quaternion.LookRotation(correctedTarget - head.position), Time.deltaTime * 5);

            // Only start lasering if we're pointing the target
            Vector3 directionToTarget = (target.transform.position - head.transform.position).normalized;
            Vector3 forwardDirection = head.transform.forward;
            float dotProduct = Vector3.Dot(forwardDirection, directionToTarget);
            // Normalised dot product will give a resultant vector magnitude determining if the turret is actually facing
            // the target. If this isn't the case we can wait for it to be true as the lerp above will move us there so we
            // can early return
            if (dotProduct < 0.9)
            {
                // Still moving to look at target, make sure to disable everything otherwise we might leave
                // phantom remnants from a previous laser beam
                laserSound.Stop();
                laserRenderer.enabled = false;
                return;
            }

            // Do some lasering
            if (!laserSound.isPlaying)
                laserSound.Play();

            // Draw the laser straight forward to pierce through everything until we hit something like a wall
            // Max length of 8 units
            Vector3 laserEndPosition = fire.position + fire.forward * 8;
            Vector3 rayOrigin = fire.position;
            float remainingDistance = 8f;

            // Continually raycast until we hit the wall or run out of distance
            while (remainingDistance > 0)
            {
                if (Physics.Raycast(rayOrigin, fire.forward, out RaycastHit hit, remainingDistance))
                {
                    if (hit.collider.CompareTag("Enemy"))
                    {
                        hit.transform.gameObject.TryGetComponent(out Health health);
                        health?.TakeDamage(damage * Time.deltaTime);
                        // Continue the raycast from the hit point
                        remainingDistance -= hit.distance;
                        // Add a slight offset to avoid hitting the same enemy again
                        rayOrigin = hit.point + fire.forward * 0.01f;
                    }
                    else
                    {
                        // We've hit the wall
                        laserEndPosition = hit.point;
                        break;
                    }
                }
                else
                {
                    laserEndPosition = fire.position + fire.forward * remainingDistance;
                    break;
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