using Entity.Types;
using UnityEngine;

namespace Entity.Markers
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
    }
}