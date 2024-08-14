using System.Linq;
using TMPro;
using UnityEngine;

namespace MainMenu
{
    public class CumulTime : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timeText;

        internal void Awake()
        {
            // Display the millis like we usually would for a level, but use a sum of all completed levels
            int sumMillis = SaveFile.Load().completedLevelTimesMillis.Sum();
            timeText.text = $"{sumMillis / 60000:D2}:{sumMillis % 60000 / 1000:D2}.{sumMillis % 1000:D3}";
        }
    }
}