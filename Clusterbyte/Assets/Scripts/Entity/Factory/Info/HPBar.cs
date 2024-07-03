using UnityEngine;

namespace Entity.Factory.Info
{
    public class HPBar : Entity
    {
        public HPBar(GameObject prefab) : base("HpBar", prefab)
        {
            // HPBars are the same for all enemies
        }
    }
}