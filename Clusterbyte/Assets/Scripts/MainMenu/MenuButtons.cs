using UnityEngine;
using UnityEngine.SceneManagement;

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

        public void OnHowToPlayClicked()
        {
            SceneManager.LoadScene("How To Play");
        }

        public void OnQuitButtonClicked()
        {
            Application.Quit();
        }
    }
}
