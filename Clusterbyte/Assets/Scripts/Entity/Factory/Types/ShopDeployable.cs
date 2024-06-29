using UnityEngine;

namespace Entity.Factory.Types
{
    public class ShopDeployable : Entity
    {
        public ShopDeployable(string name, GameObject prefab, int cost) : base(name, prefab)
        {
            this.cost = cost;
        }

        public int cost { get; private set; }
    }
}