using UnityEngine;

namespace Entity.Factory
{
    public abstract class Entity
    {
        protected Entity(string name, GameObject prefab)
        {
            this.name = name;
            this.prefab = prefab;
        }

        public GameObject prefab { get; }
        public string name { get; private set; }

        public GameObject SpawnAtGrid(float x, float y)
        {
            GameObject o = Object.Instantiate(prefab, new Vector3(x, 0.5f, y), Quaternion.identity);
            OnSpawn(o);
            return o;
        }

        protected virtual void OnSpawn(GameObject spawnedObject)
        {
        }
    }
}