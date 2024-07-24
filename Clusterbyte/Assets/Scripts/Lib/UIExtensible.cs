using System.Collections.Generic;
using UnityEngine;

namespace Lib
{
    /// <summary>
    /// Marker class for a UI MonoBehaviour that can be shown and hidden.
    /// </summary>
    public abstract class UIExtensible : MonoBehaviour
    {
        private static readonly List<UIExtensible> instances = new();

        /// <summary>
        /// Clear all stored instantiations of UIExtensible.
        /// </summary>
        public static void clearInstances()
        {
            instances.Clear();
        }

        /// <summary>
        /// Hide all UI elements that are currently being shown.
        /// </summary>
        public static void HideAll()
        {
            foreach (UIExtensible instance in instances)
            {
                instance.Hide();
            }
        }

        /// <summary>
        /// Show all UI elements that are currently hidden.
        /// </summary>
        public static void ShowAll()
        {
            foreach (UIExtensible instance in instances)
            {
                instance.Show();
            }
        }

        protected UIExtensible()
        {
            instances.Add(this);
        }

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