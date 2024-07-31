using Lib;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class WonUI : UIExtensible
    {
        [SerializeField] private TextMeshProUGUI timeText;

        public override void Show()
        {
            base.Show();
            timeText.text = $"in {GameManager.instance.timeInState:F2} seconds";
        }

        public void OnMenuPressed()
        {
            SceneManager.LoadScene("Main Menu");
        }
    }
}