using Entity.Factory.Types;
using UnityEngine;

namespace Entity.Factory.Info
{
    public class StartMarker : MapElement
    {
        public StartMarker(GameObject rootObject) : base("StartMarker", rootObject)
        {
        }

        protected override void OnSpawn(GameObject o)
        {
            o.GetComponent<Renderer>().material.color = Color.green;
        }

        public override object Clone()
        {
            return new StartMarker(prefab);
        }
    }
}