using UnityEngine;

namespace Lib
{
    /// <summary>
    /// Marker class for a UI MonoBehaviour that can be shown and hidden.
    /// </summary>
    public abstract class UIExtensible : MonoBehaviour
    {
        /// <summary>
        /// Called when this UI element should stop rendering.
        /// </summary>
        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Called when this UI element should start rendering.
        /// </summary>
        public virtual void Show()
        {
            gameObject.SetActive(true);
        }
    }
}