using UnityEngine;
using UnityEngine.SceneManagement;

namespace MainMenu
{
    public class ReturnToMainMenu : MonoBehaviour
    {
        public void OnBackPressed()
        {
            Clusterbyte.QUEUED_LEVEL = null;
            SceneManager.LoadScene("Main Menu");
        }
    }
}