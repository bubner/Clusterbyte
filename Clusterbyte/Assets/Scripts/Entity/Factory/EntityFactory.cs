using System.Collections.Generic;
using Entity.Factory.Enemy;
using Entity.Factory.Info;
using Entity.Factory.Types;
using UnityEngine;

namespace Entity.Factory
{
    /// <summary>
    /// A static factory class that provides access to all configured entities in the game.
    /// </summary>
    public class EntityFactory : MonoBehaviour
    {
        private static readonly List<Entity> entities = new();

        [SerializeField] private GameObject turretPrefab;
        [SerializeField] private GameObject laserBlasterPrefab;
        [SerializeField] private GameObject nukerPrefab;
        [SerializeField] private GameObject placeableTerrainPrefab;
        [SerializeField] private GameObject deadTerrainPrefab;
        [SerializeField] private GameObject genericMarkerPrefab;
        [SerializeField] private GameObject blobPrefab;
        [SerializeField] private GameObject dronePrefab;
        [SerializeField] private GameObject followingHpBarPrefab;
        [SerializeField] private GameObject popupPrefab;

        internal void Awake()
        {
            entities.Add(new MapElement("Dead", deadTerrainPrefab));
            entities.Add(new MapElement("Placeable", placeableTerrainPrefab));
            entities.Add(new StartMarker(genericMarkerPrefab));
            entities.Add(new EndMarker(genericMarkerPrefab));
            entities.Add(new HPBar(followingHpBarPrefab));
            entities.Add(new Popup(popupPrefab));

            entities.Add(new ShopDeployable("Turret", turretPrefab, 5, "Locks onto the closest enemy and shoots at it. Shells deal Area-of-Effect damage. Normal radius."));
            entities.Add(new ShopDeployable("Laser Blaster", laserBlasterPrefab, 15, "Locks onto the closest enemy and continuously deals damage in a piercing beam that can hurt multiple enemies. Large radius."));
            entities.Add(new ShopDeployable("Nuker", nukerPrefab, 30, "Destroyer of all existing things. Summons a nuke after charging, dealing significant damage. Loud. Requires at least three enemies to be in range."));

            entities.Add(new Mazeable("Blob", blobPrefab));
            entities.Add(new Mazeable("Drone", dronePrefab));
        }

        /// <summary>
        /// Retrieves an entity by its name.
        /// </summary>
        /// <param name="entityName">The `name` of the entity</param>
        /// <typeparam name="T">The type of the entity</typeparam>
        /// <returns>An entity that matches this name, casted to T otherwise null if this entity could not be found</returns>
        public static T Get<T>(string entityName) where T : Entity
        {
            Entity entity = entities.Find(entity => entity.name == entityName);
            if (entity != null)
                return (T)entity.Clone();
            Debug.LogError($"Entity {entityName} not found.");
            return null;
        }

        /// <summary>
        /// Retrieves an entity by its type.
        /// </summary>
        /// <typeparam name="T">The type of this entity.</typeparam>
        /// <returns>An entity that matches this type, or null</returns>
        public static T Get<T>() where T : Entity
        {
            Entity entity = entities.Find(entity => entity.GetType() == typeof(T));
            if (entity != null)
                return (T)entity.Clone();
            Debug.LogError("Entity not found.");
            return null;
        }

        /// <summary>
        /// Tries to retrieve an entity by its type.
        /// </summary>
        /// <param name="obj">The entity that was found, or null</param>
        /// <typeparam name="T">The type of the entity</typeparam>
        /// <returns>True if the entity was found, false otherwise.</returns>
        public static bool TryGet<T>(out Entity obj)
        {
            Entity entity = entities.Find(entity => entity.GetType() == typeof(T));
            if (entity != null)
            {
                obj = entity.Clone() as Entity;
                return true;
            }

            Debug.LogError("Entity not found.");
            obj = null;
            return false;
        }

        /// <summary>
        /// Tries to retrieve an entity by its name.
        /// </summary>
        /// <param name="entityName">The `name` of the entity</param>
        /// <param name="obj">The entity that was found, or null</param>
        /// <typeparam name="T">The type of the entity</typeparam>
        /// <returns>True if the entity was found, false otherwise.</returns>
        public static bool TryGet<T>(string entityName, out T obj) where T : Entity
        {
            Entity entity = entities.Find(entity => entity.name == entityName);
            if (entity is T e)
            {
                obj = e.Clone() as T;
                return true;
            }

            Debug.LogError($"Entity {entityName} not found or not of the expected type.");
            obj = null;
            return false;
        }
    }
}