using System;
using UnityEngine;

namespace Viewport
{
    public class CameraPositioner : MonoBehaviour
    {
        [SerializeField] private Transform settingSpot;
        [SerializeField] private Transform activeSpot;
        private Transform observingSpot;
        private CameraSpot currentState;
        public float interpolateSpeed = 2;

        public enum CameraSpot
        {
            SETTING,
            ACTIVE,
            OBSERVING
        }

        public void SetObservingSpot(Vector3 pos, Quaternion rot)
        {
            if (observingSpot != null)
            {
                Destroy(observingSpot.gameObject);
            }
            observingSpot = new GameObject("Observing Spot").transform;
            observingSpot.position = pos;
            observingSpot.rotation = rot;
        }

        /// <summary>
        /// Set the position of the camera to the specified spot
        /// </summary>
        /// <param name="spot">Camera Position</param>
        public void SetPosition(CameraSpot spot)
        {
            // Observing spot must be set first
            if (spot == CameraSpot.OBSERVING && observingSpot == null)
            {
                Debug.LogError("Observing spot must be set first before setting the camera to observe it.");
                return;
            }

            currentState = spot;
        }

        internal void Update()
        {
            Transform spot = currentState switch
            {
                CameraSpot.SETTING => settingSpot,
                CameraSpot.ACTIVE => activeSpot,
                CameraSpot.OBSERVING => observingSpot,
                _ => throw new ArgumentOutOfRangeException()
            };
            transform.position = Vector3.Lerp(transform.position, spot.position, Time.deltaTime * interpolateSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, spot.rotation, Time.deltaTime * interpolateSpeed);
        }
    }
}
