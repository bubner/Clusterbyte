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
        internal abstract void Hide();

        /// <summary>
        /// Called when this UI element should start rendering.
        /// </summary>
        internal abstract void Show();
    }
}