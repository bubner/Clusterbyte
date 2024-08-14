using System;
using UnityEngine;

namespace Viewport
{
    /// <summary>
    /// Component for the camera to move to different pre-defined positions at different times.
    /// </summary>
    public class CameraPositioner : MonoBehaviour
    {
        [SerializeField] private Transform settingSpot;
        [SerializeField] private Transform activeSpot;
        private Transform observingSpot;
        private CameraSpot currentState;
        /// <summary>
        /// How fast the camera should move to the new position.
        /// </summary>
        public float interpolateSpeed = 2;

        /// <summary>
        /// The different camera positions that the camera can be set to.
        /// </summary>
        public enum CameraSpot
        {
            ANGLED,
            OVERHEAD,
            CUSTOM
        }

        /// <summary>
        /// Set the position of the camera that will be used when the camera is set to the "CUSTOM" position.
        /// </summary>
        /// <param name="pos">the position in world space</param>
        /// <param name="rot">the rotation of the camera</param>
        public void SetCustomSpot(Vector3 pos, Quaternion rot)
        {
            if (observingSpot != null)
            {
                Destroy(observingSpot.gameObject);
            }
            observingSpot = new GameObject("Observing Spot").transform;
            observingSpot.position = pos;
            observingSpot.rotation = rot;
        }

        public Transform GetCustomSpot()
        {
            return observingSpot;
        }

        /// <summary>
        /// Set the position of the camera to the specified spot
        /// </summary>
        /// <param name="spot">Camera Position</param>
        public void SetPosition(CameraSpot spot)
        {
            // Observing spot must be set first
            if (spot == CameraSpot.CUSTOM && observingSpot == null)
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
                CameraSpot.ANGLED => settingSpot,
                CameraSpot.OVERHEAD => activeSpot,
                CameraSpot.CUSTOM => observingSpot,
                _ => throw new ArgumentOutOfRangeException()
            };
            // Lerp over to the new spot, could use a SmoothDamp but this effect looks more snappy
            transform.position = Vector3.Lerp(transform.position, spot.position, Time.deltaTime * interpolateSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, spot.rotation, Time.deltaTime * interpolateSpeed);
        }
    }
}
