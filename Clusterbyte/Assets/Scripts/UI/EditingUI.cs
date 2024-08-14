using Entity.Factory;
using Entity.Factory.Info;
using Entity.Factory.Types;
using Lib;
using TMPro;
using UnityEngine;

namespace UI
{
    public class EditingUI : UIExtensible
    {
        [SerializeField] private TextMeshProUGUI refundText;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        private ShopDeployable deployable;

        public void SetItem(ShopDeployable item)
        {
            deployable = item;
            nameText.text = deployable.name;
            descriptionText.text = deployable.description;
            refundText.text = $"Refund {deployable.cost} tokens?";
        }

        public void OnRefundClicked()
        {
            if (deployable == null) return;
            EntityFactory.Get<Popup>()
                .SendText($"Refunded {deployable.name} for {deployable.cost} tokens.", Popup.Type.INFO);
            Destroy(deployable.instance);
            GameManager.instance.playerStats.AddTokens(deployable.cost);
            GameManager.instance.deployed.Remove(deployable);
            GameManager.instance.SetState(GameManager.VIEWING);
            deployable = null;
        }
    }
}