using Entity.Factory.Types;
using UnityEngine;

namespace Entity.Factory.Info
{
    /// <summary>
    /// The red end marker of the maze.
    /// </summary>
    public class EndMarker : MapElement
    {
        public EndMarker(GameObject rootObject) : base("EndMarker", rootObject)
        {
            // Will take in a general prefab that we need to change the colour of
        }

        protected override void OnSpawn(GameObject o)
        {
            o.GetComponent<Renderer>().material.color = Color.red;
        }

        public override object Clone()
        {
            return new EndMarker(prefab);
        }
    }
}