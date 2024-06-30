using UnityEngine;
using UnityEngine.SceneManagement;

namespace MainMenu
{
    /// <summary>
    /// Clusterbyte Main menu button handlers.
    /// </summary>
    public class MenuButtons : MonoBehaviour
    {
        public void OnPlayButtonClicked()
        {
            SceneManager.LoadScene("Game");
        }

        public void OnSettingsButtonClicked()
        {
            // TODO
        }

        public void OnQuitButtonClicked()
        {
            Application.Quit();
        }
    }
}
