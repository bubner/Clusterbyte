using Entity.Factory;
using Entity.Factory.Markers;
using Lib;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Entity.Behaviours.Enemy
{
    public class HealthMarker : UIExtensible
    {
        [SerializeField] private Health health;
        private Camera cam;
        private RectTransform translation;
        private Slider bar;
        private TextMeshProUGUI text;

        internal void Start()
        {
            cam = Camera.main;
            GameObject obj = Instantiate(EntityFactory.Get<HPBar>().prefab, transform);
            bar = obj.GetComponentInChildren<Slider>();
            text = obj.GetComponentInChildren<TextMeshProUGUI>();
            translation = obj.GetComponentInChildren<RectTransform>();
        }

        internal void Update()
        {
            text.text = $"{health.health:F0}/{health.startingHealth}";
            bar.value = health.health / health.startingHealth;
            // TODO: positioning of the health bar
        }

        public override void Show()
        {
            bar.gameObject.SetActive(true);
        }

        public override void Hide()
        {
            bar.gameObject.SetActive(false);
        }
    }
}