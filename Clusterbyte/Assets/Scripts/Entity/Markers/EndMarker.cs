using Entity.Types;
using UnityEngine;

namespace Entity.Markers
{
    public class EndMarker : MapElement
    {
        public EndMarker(GameObject rootObject) : base("EndMarker", rootObject)
        {
        }

        protected override void OnSpawn(GameObject o)
        {
            o.GetComponent<Renderer>().material.color = Color.red;
        }
    }
}