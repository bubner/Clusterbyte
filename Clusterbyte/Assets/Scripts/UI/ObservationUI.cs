using Lib;
using UnityEngine;

namespace UI
{
    public class ObservationUI : UIExtensible
    {
        public void OnBackPressed()
        {
            GameManager.instance.SetState(GameManager.SETTING);
        }

        internal override void Hide()
        {
            gameObject.SetActive(false);
        }

        internal override void Show()
        {
            gameObject.SetActive(true);
        }
    }
}
