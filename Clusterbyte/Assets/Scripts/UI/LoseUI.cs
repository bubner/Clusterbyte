using Lib;
using UnityEngine.SceneManagement;

namespace UI
{
    public class LoseUI : UIExtensible
    {
        public void OnRetryPressed()
        {
            // TODO: leave and global lives system
            // Reload current scene as everything is still configured correctly
            Clusterbyte.QUEUED_LEVEL = GameManager.instance.currentLevel;
            SceneManager.LoadScene("Game");
        }

        public void OnMainMenuPressed()
        {
            SceneManager.LoadScene("Main Menu");
        }
    }
}