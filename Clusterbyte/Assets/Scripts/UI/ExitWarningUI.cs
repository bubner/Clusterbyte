using System.Linq;
using Lib;
using TMPro;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// UI for when the user presses Escape in the ACTIVE phase.
    /// </summary>
    public class ExitWarningUI : UIExtensible
    {
        [SerializeField] private TextMeshProUGUI lifeCount;

        private int livesToRemove;

        public override void Show()
        {
            base.Show();
            livesToRemove = Clusterbyte.ENEMY_SPAWN_TIMES[GameManager.instance.currentLevel].Length;
            lifeCount.text = livesToRemove.ToString();
        }

        public void OnCancelPressed()
        {
            Hide();
        }

        public void OnConfirmPressed()
        {
            GameManager manager = GameManager.instance;
            for (int i = livesToRemove; i > 0; i--)
            {
                manager.playerStats.LoseLife();
            }
            if (manager.playerStats.lives > 0)
                manager.SetState(GameManager.VIEWING);
            Hide();
        }
    }
}