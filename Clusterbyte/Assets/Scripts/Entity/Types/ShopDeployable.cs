using UnityEngine;

namespace Entity.Types
{
    public class ShopDeployable : Entity
    {
        public int cost { get; private set; }

        public ShopDeployable(string name, GameObject prefab, int cost) : base(name, prefab)
        {
            this.cost = cost;
        }
    }
}