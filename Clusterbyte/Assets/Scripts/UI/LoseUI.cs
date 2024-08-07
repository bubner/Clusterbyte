using Lib;
using UnityEngine.SceneManagement;

namespace UI
{
    public class LoseUI : UIExtensible
    {
        public void OnMainMenuPressed()
        {
            SceneManager.LoadScene("Main Menu");
        }
    }
}