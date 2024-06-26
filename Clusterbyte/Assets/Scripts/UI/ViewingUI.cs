using Lib;

namespace UI
{
    public class ViewingUI : UIExtensible
    {
        public void OnStartPressed()
        {
            GameManager.instance.SetState(GameManager.ACTIVE);
        }
    }
}