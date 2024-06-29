using Entity.Factory.Types;
using UnityEngine;

namespace Entity.Factory.Markers
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