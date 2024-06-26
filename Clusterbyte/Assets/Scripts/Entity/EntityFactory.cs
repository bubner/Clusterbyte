using System.Collections.Generic;
using Entity.Enemy;
using Entity.Markers;
using Entity.Types;
using UnityEngine;

namespace Entity
{
    public class EntityFactory : MonoBehaviour
    {
        [SerializeField] private GameObject turretPrefab;
        [SerializeField] private GameObject placeableTerrainPrefab;
        [SerializeField] private GameObject deadTerrainPrefab;
        [SerializeField] private GameObject genericMarkerPrefab;
        [SerializeField] private GameObject blobPrefab;

        private static readonly List<Entity> entities = new();

        internal void Awake()
        {
            entities.Add(new MapElement("Dead", deadTerrainPrefab));
            entities.Add(new MapElement("Placeable", placeableTerrainPrefab));
            entities.Add(new StartMarker(genericMarkerPrefab));
            entities.Add(new EndMarker(genericMarkerPrefab));

            entities.Add(new ShopDeployable("Turret", turretPrefab, 5));
            entities.Add(new Mazeable("Blob", blobPrefab));
        }
        
        public static T Get<T>(string entityName) where T : Entity
        {
            Entity entity = entities.Find(entity => entity.name == entityName);
            if (entity != null)
                return (T)entity;
            Debug.LogError($"Entity {entityName} not found.");
            return null;
        }

        public static T Get<T>() where T : Entity
        {
            Entity entity = entities.Find(entity => entity.GetType() == typeof(T));
            if (entity != null)
                return (T)entity;
            Debug.LogError("Entity not found.");
            return null;
        }
        
        public static bool TryGet<T>(out Entity obj)
        {
            Entity entity = entities.Find(entity => entity.GetType() == typeof(T));
            if (entity != null)
            {
                obj = entity;
                return true;
            }
            Debug.LogError("Entity not found.");
            obj = null;
            return false;
        }
        
        public static bool TryGet<T>(string entityName, out T obj) where T : Entity
        {
            Entity entity = entities.Find(entity => entity.name == entityName);
            if (entity is T e)
            {
                obj = e;
                return true;
            }
            Debug.LogError($"Entity {entityName} not found or not of the expected type.");
            obj = null;
            return false;
        }
    }
}