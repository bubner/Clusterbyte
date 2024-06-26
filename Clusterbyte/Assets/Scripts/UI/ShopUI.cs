using Lib;

namespace UI
{
    public class ShopUI : UIExtensible
    {
        public void OnBackPressed()
        {
            GameManager.instance.SetState(GameManager.VIEWING);
        }
    }
}
