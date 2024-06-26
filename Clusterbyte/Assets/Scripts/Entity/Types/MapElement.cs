using UnityEngine;

namespace Entity.Types
{
    public class MapElement : Entity
    {
        public MapElement(string name, GameObject prefab) : base(name, prefab)
        {
            prefab.name = "Terrain" + name;
        }
    }
}