using UnityEngine;

namespace Entity.Factory.Enemy
{
    /// <summary>
    /// A generic enemy that can be placed in the maze.
    /// </summary>
    public class Mazeable : Entity
    {
        public Mazeable(string name, GameObject prefab) : base(name, prefab)
        {
            // name will be used when spawning as a key to the dictionary in the level factory
        }

        public override object Clone()
        {
            return new Mazeable(name, prefab);
        }
    }
}