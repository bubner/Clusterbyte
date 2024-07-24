using Lib;
using TMPro;
using UnityEngine;

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
    }
}