using UnityEngine;
using UnityEngine.Assertions;

namespace Entity.Behaviours.Placeables.Nuker
{
    public class CameraShaker : MonoBehaviour
    {
        /// <summary>
        /// How much chaos shall ensue.
        /// </summary>
        public float shakeAmount = 2;

        private Camera cam;
        private Vector3 holdingPosition;
        private Quaternion holdingAngle;

        internal void Awake()
        {
            cam = Camera.main;
            Assert.IsNotNull(cam);
            holdingPosition = cam.transform.position;
            holdingAngle = cam.transform.rotation;
        }

        internal void Update()
        {
            cam.transform.position = new Vector3(
                holdingPosition.x + Random.Range(-shakeAmount, shakeAmount),
                holdingPosition.y + Random.Range(-shakeAmount, shakeAmount),
                holdingPosition.z + Random.Range(-shakeAmount, shakeAmount)
            );

            cam.transform.rotation = Quaternion.Euler(
                    holdingAngle.eulerAngles.x + Random.Range(-shakeAmount, shakeAmount),
                    holdingAngle.eulerAngles.y + Random.Range(-shakeAmount, shakeAmount),
                    holdingAngle.eulerAngles.z + Random.Range(-shakeAmount, shakeAmount)
            );
        }
    }
}