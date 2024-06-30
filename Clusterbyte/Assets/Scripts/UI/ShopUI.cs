using Lib;

namespace UI
{
    /// <summary>
    /// Shop UI for when the player clicks on a green terrain tile.
    /// </summary>
    public class ShopUI : UIExtensible
    {
        public void OnBackPressed()
        {
            GameManager.instance.SetState(GameManager.VIEWING);
        }
    }
}
