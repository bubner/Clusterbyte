using UnityEngine;

namespace Viewport
{
    public class MouseNoiseMaker : MonoBehaviour
    {
        private AudioSource audioSrc;
        
        internal void Awake()
        {
            TryGetComponent(out audioSrc);
        }

        internal void Update()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                audioSrc.Play();
            }
        }
    }
}
