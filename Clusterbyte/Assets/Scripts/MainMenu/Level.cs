using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainMenu
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private int level;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI completionText;

        internal void Start()
        {
            levelText.text = "Level " + level;
            completionText.text = "TODO"; // TODO
            button.onClick.AddListener(() =>
            {
                Clusterbyte.QUEUED_LEVEL = level;
                SceneManager.LoadScene("Game");
            });
        }
    }
}