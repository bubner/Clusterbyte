using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainMenu
{
    /// <summary>
    /// Game Selector interactivity for the main menu.
    /// </summary>
    public class GameSelector : MonoBehaviour
    {
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private TextMeshProUGUI livesLeftText;
        [SerializeField] private TextMeshProUGUI resetButtonText;
        [SerializeField] private Image resetButtonImage;

        private bool confirming;

        internal void Start()
        {
            SaveFile.Save save = SaveFile.Load();
            if (save.lives <= 0)
            {
                livesLeftText.transform.parent.gameObject.GetComponent<Image>().color = Color.red;
            }
            livesLeftText.text = $"Lives left: {Mathf.Max(0, save.lives)}";
            confirming = false;
        }

        public void OnBackSelected()
        {
            gameObject.SetActive(false);
            mainMenuPanel.SetActive(true);
            confirming = false;
            resetButtonText.text = "Reset";
            resetButtonText.color = Color.white;
            resetButtonImage.color = new Color(197 / 255f, 29 / 255f, 0 / 255f, 221 / 255f);
        }

        public void OnResetPressed()
        {
            if (!confirming)
            {
                confirming = true;
                resetButtonText.text = "Confirm?";
                resetButtonText.color = Color.black;
                resetButtonImage.color = Color.green;
                return;
            }
            SaveFile.Reset();
            // Have to reload the scene to invoke the Start method of each Level object
            SceneManager.LoadScene("Main Menu");
        }
    }
}