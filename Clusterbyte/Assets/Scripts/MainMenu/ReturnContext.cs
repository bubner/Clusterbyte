using UnityEngine;

namespace MainMenu
{
    /// <summary>
    /// Return to the Level Selection screen if we're coming from a level, otherwise return to the main menu.
    /// </summary>
    public class ReturnContext : MonoBehaviour
    {
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private GameObject gameSelectionPanel;

        internal void Start()
        {
            if (Clusterbyte.QUEUED_LEVEL == null)
            {
                mainMenuPanel.SetActive(true);
            }
            else
            {
                gameSelectionPanel.SetActive(true);
            }
        }
    }
}
