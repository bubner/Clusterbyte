using UnityEngine;

namespace MainMenu
{
    /// <summary>
    /// Clusterbyte Main menu button handlers.
    /// </summary>
    public class MenuButtons : MonoBehaviour
    {
        [SerializeField] private GameObject gameSelectionPanel;

        public void OnPlayButtonClicked()
        {
            transform.parent.gameObject.SetActive(false);
            gameSelectionPanel.SetActive(true);
        }

        public void OnQuitButtonClicked()
        {
            Application.Quit();
        }
    }
}
