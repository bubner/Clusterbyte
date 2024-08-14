using UnityEngine;

namespace Entity.Factory.Types
{
    public class MapElement : Entity
    {
        public MapElement(string name, GameObject prefab) : base(name, prefab)
        {
            prefab.name = "Terrain" + name;
        }

        public override object Clone()
        {
            return new MapElement(name, prefab);
        }
    }
}