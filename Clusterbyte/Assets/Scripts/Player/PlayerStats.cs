using Lib;
using TMPro;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// Token and life tracking for the player.
    /// </summary>
    public class PlayerStats : UIExtensible
    {
        [SerializeField] private GameObject hud;
        [SerializeField] private TextMeshProUGUI tokenText;
        [SerializeField] private TextMeshProUGUI lifeText;

        public int tokens { get; private set; }
        public int lives { get; private set; }

        /// <summary>
        /// Reset the player's stats to their starting values.
        /// </summary>
        public void SetStats(int startingTokens, int startingLives)
        {
            tokens = startingTokens;
            lives = startingLives;
        }

        /// <summary>
        /// Attempt to buy something with the player's tokens.
        /// </summary>
        /// <param name="amount">The amount that should be deducted</param>
        /// <returns>Whether the transaction was successful</returns>
        public bool TryTransaction(int amount)
        {
            if (tokens - amount < 0)
            {
                return false;
            }

            tokens -= amount;
            return true;
        }

        /// <summary>
        /// Remove a life from the player.
        /// </summary>
        public void LoseLife()
        {
            lives--;
        }

        /// <summary>
        /// Add tokens to the player's inventory.
        /// </summary>
        /// <param name="amount">The amount to add</param>
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
            tokenText.text = "Level Tokens: " + (tokens > 100 ? "\u221e" : tokens);
            lifeText.text = "Lives: " + Mathf.Max(0, lives);
        }
    }
}