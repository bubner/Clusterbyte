using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainMenu
{
    /// <summary>
    /// Level button selector and completion text.
    /// </summary>
    public class Level : MonoBehaviour
    {
        [SerializeField] private int level;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI completionText;

        internal void Start()
        {
            levelText.text = "Level " + level;
            SaveFile.Save saveFile = SaveFile.Load();
            if (saveFile.completedLevelTimesMillis[level - 1] > 0)
            {
                GetComponent<Image>().color = new Color(85 / 255f, 255 / 255f, 74 / 255f, 100 / 255f);
                int millis = saveFile.completedLevelTimesMillis[level - 1];
                completionText.text = $"{millis / 60000:D2}:{millis % 60000 / 1000:D2}.{millis % 1000:D3}";
            }
            else
            {
                GetComponent<Image>().color = new Color(255 / 255f, 85 / 255f, 74 / 255f, 100 / 255f);
                completionText.text = "Not completed";
            }
            if (saveFile.lives <= 0)
            {
                button.enabled = false;
                button.image.color = Color.gray;
                return;
            }
            button.onClick.AddListener(() =>
            {
                Clusterbyte.QUEUED_LEVEL = level;
                SceneManager.LoadScene("Game");
            });
        }
    }
}