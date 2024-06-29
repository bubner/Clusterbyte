using TMPro;
using UnityEngine;

namespace MainMenu
{
    public class SetVersion : MonoBehaviour
    {
        internal void Awake()
        {
            GetComponent<TextMeshProUGUI>().text = Application.version;
        }
    }
}
