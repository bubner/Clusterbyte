using UnityEngine;

namespace Entity.Factory
{
    /// <summary>
    /// An entity with prefab and name description that can be instantiated in the game world.
    /// </summary>
    public abstract class Entity
    {
        protected Entity(string name, GameObject prefab)
        {
            this.name = name;
            this.prefab = prefab;
        }

        /// <summary>
        /// The prefab of this entity.
        /// </summary>
        public GameObject prefab { get; }
        /// <summary>
        /// The name of this entity to use when finding this object in EntityFactory.
        /// </summary>
        public string name { get; private set; }
        /// <summary>
        /// The instance of this entity that was last spawned. Will be null if no instance was spawned through
        /// the methods of this class.
        /// </summary>
        public GameObject instance { get; private set; }

        /// <summary>
        /// Instantiates this entity at the given position in terms of grid coordinates.
        /// </summary>
        /// <param name="x">x (horizontal) component</param>
        /// <param name="y">y (vertical) component</param>
        /// <param name="parent">Optional parent for this entity when spawning</param>
        /// <returns></returns>
        public GameObject InstantiateAtGrid(float x, float y, Transform parent = null)
        {
            return Instantiate(new Vector3(x, 0.5f, y), Quaternion.identity, parent);
        }

        /// <summary>
        /// Instantiates this entity at the given position and rotation.
        /// </summary>
        /// <param name="position">the position to instantiate</param>
        /// <param name="rotation">the rotation to give the instantiated object</param>
        /// <param name="parent">Optional parent for this entity when spawning</param>
        /// <returns></returns>
        public GameObject Instantiate(Vector3 position, Quaternion rotation, Transform parent = null)
        {
            GameObject o = parent
                ? Object.Instantiate(prefab, position, rotation, parent)
                : Object.Instantiate(prefab, position, rotation);
            instance = o;
            OnSpawn(o);
            return o;
        }

        /// <summary>
        /// Called when this Entity is spawned in the game world through an Entity method.
        /// </summary>
        /// <param name="spawnedObject">the object (not prefab) that was spawned</param>
        protected virtual void OnSpawn(GameObject spawnedObject)
        {
        }
    }
}