using Lib;
using TMPro;
using UnityEngine;

namespace Player
{
    public class PlayerStats : UIExtensible
    {
        [SerializeField] private GameObject hud;
        [SerializeField] private TextMeshProUGUI tokenText;

        public int startingTokens = 5;
        public int tokens { get; private set; }

        public void ResetTokens()
        {
            tokens = startingTokens;
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

        public void AddTokens(int amount)
        {
            tokens += amount;
        }

        internal override void Hide()
        {
            hud.SetActive(false);
        }

        internal override void Show()
        {
            hud.SetActive(true);
        }

        internal void Update()
        {
            tokenText.text = tokens.ToString();
        }
    }
}