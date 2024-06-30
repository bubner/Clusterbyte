using Lib;

namespace UI
{
    /// <summary>
    /// Default UI when the player is viewing the game.
    /// </summary>
    public class ViewingUI : UIExtensible
    {
        public void OnStartPressed()
        {
            GameManager.instance.SetState(GameManager.ACTIVE);
        }
    }
}