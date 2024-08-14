using UnityEngine;
using UnityEngine.UI;

namespace Entity.Factory.Info
{
    public class Popup : Entity
    {
        public Popup(GameObject prefab) : base("Popup", prefab)
        {
            // General UI popup that does not care about where it is spawned
        }

        /// <summary>
        /// The type of message that will be displayed in the popup.
        /// The colour of the popup will change depending on the type.
        /// </summary>
        public enum Type
        {
            INFO,
            SUCCESS,
            WARNING
        }

        /// <summary>
        /// Send a new popup message to the screen.
        /// </summary>
        /// <param name="text">Text that will be popped up</param>
        /// <param name="type">Type of message</param>
        public void SendText(string text, Type type)
        {
            Instantiate(Vector3.zero, Quaternion.identity, GameObject.Find("Canvas").transform);
            instance.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = text;
            instance.GetComponent<Image>().color = type switch
            {
                Type.SUCCESS => Color.green,
                Type.WARNING => Color.yellow,
                Type.INFO => Color.gray,
                _ => instance.GetComponent<Image>().color
            };
        }

        public override object Clone()
        {
            return new Popup(prefab);
        }
    }
}