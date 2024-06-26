using Lib;
using TMPro;
using UnityEngine;

namespace Player
{
    public class PlayerStats : UIExtensible
    {
        [SerializeField] private GameObject hud;
        [SerializeField] private TextMeshProUGUI tokenText;
        [SerializeField] private TextMeshProUGUI lifeText;

        public int startingTokens = 5;
        public int startingLives = 20;

        public int tokens { get; private set; }
        public int lives { get; private set; }

        public void ResetStats()
        {
            tokens = startingTokens;
            lives = startingLives;
        }

        public bool TryTransaction(int amount)
        {
            if (tokens - amount < 0)
            {
                return false;
            }

            tokens -= amount;
            return true;
        }

        public void LoseLife()
        {
            lives--;
        }

        public void AddTokens(int amount)
        {
            tokens += amount;
        }

        public override void Hide()
        {
            hud.SetActive(false);
        }

        public override void Show()
        {
            hud.SetActive(true);
        }

        internal void Update()
        {
            tokenText.text = tokens + " tokens";
            lifeText.text = lives + " lives";
        }
    }
}