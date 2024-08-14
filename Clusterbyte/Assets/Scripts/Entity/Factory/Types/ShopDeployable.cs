using UnityEngine;

namespace Entity.Factory.Types
{
    public class ShopDeployable : Entity
    {
        public ShopDeployable(string name, GameObject prefab, int cost, string description) : base(name, prefab)
        {
            this.description = description;
            this.cost = cost;
        }

        public string description { get; private set; }
        public int cost { get; private set; }

        public override object Clone()
        {
            return new ShopDeployable(name, prefab, cost, description);
        }
    }
}