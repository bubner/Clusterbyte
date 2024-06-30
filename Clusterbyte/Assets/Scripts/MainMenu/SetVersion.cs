using TMPro;
using UnityEngine;

namespace MainMenu
{
    /// <summary>
    /// Update the version text on the main menu.
    /// </summary>
    public class SetVersion : MonoBehaviour
    {
        internal void Awake()
        {
            GetComponent<TextMeshProUGUI>().text = Application.version;
        }
    }
}
