using Entity.Behaviours.Enemy;
using UnityEngine;

namespace Entity.Factory.Enemy
{
    public class Mazeable : Entity
    {
        public Mazeable(string name, GameObject prefab) : base(name, prefab)
        {
        }
    }
}