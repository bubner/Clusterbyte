using Entity.Factory;
using Entity.Factory.Info;
using Lib;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Entity.Behaviours.Enemy
{
    /// <summary>
    /// Following Canvas element that displays the health of an entity.
    /// </summary>
    public class HealthMarker : UIExtensible
    {
        [SerializeField] private Health health;
        private GameObject obj;
        private Image background;
        private Slider bar;
        private TextMeshProUGUI text;

        internal void Start()
        {
            // Create a new child of the current GameObject to be the health marker
            obj = Instantiate(EntityFactory.Get<HPBar>().prefab, transform);

            bar = obj.GetComponentInChildren<Slider>();
            background = bar.GetComponentsInChildren<Image>()[1];
            text = obj.GetComponentInChildren<TextMeshProUGUI>();
        }

        internal void Update()
        {
            // Health display
            text.text = $"{health.health:F0}/{health.startingHealth}";
            bar.value = health.health / health.startingHealth;
            background.color = Color.Lerp(Color.red, Color.green, bar.value);

            // Position slightly above the enemy and rotate it upwards for the camera to see
            obj.transform.position = transform.position + Vector3.up * 2;
            obj.transform.rotation = Quaternion.Euler(90, 0, 0);
        }

        public override void Show()
        {
            obj.SetActive(true);
        }

        public override void Hide()
        {
            obj.SetActive(false);
        }
    }
}