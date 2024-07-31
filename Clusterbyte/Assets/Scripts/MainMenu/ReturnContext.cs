using UnityEngine;

namespace MainMenu
{
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
