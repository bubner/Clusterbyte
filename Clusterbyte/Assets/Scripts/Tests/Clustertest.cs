using NUnit.Framework;
using UnityEngine;

namespace Tests
{
    public class Clustertest
    {
        [Test]
        public void TileIndexing()
        {
            // Individual corner x
            Assert.AreEqual(-9, Clusterbyte.SpawnAtTile(1, 1).x);
            Assert.AreEqual(9, Clusterbyte.SpawnAtTile(18, 1).x);
            Assert.AreEqual(9, Clusterbyte.SpawnAtTile(18, 10).x);
            Assert.AreEqual(-9, Clusterbyte.SpawnAtTile(1, 10).x);
            // y
            Assert.AreEqual(-5, Clusterbyte.SpawnAtTile(1, 1).y);
            Assert.AreEqual(-5, Clusterbyte.SpawnAtTile(18, 1).y);
            Assert.AreEqual(5, Clusterbyte.SpawnAtTile(18, 10).y);
            Assert.AreEqual(5, Clusterbyte.SpawnAtTile(1, 10).y);
            // Full corner vector tests
            Assert.AreEqual(new Vector2(-9, -5), Clusterbyte.SpawnAtTile(1, 1));
            Assert.AreEqual(new Vector2(9, -5), Clusterbyte.SpawnAtTile(18, 1));
            Assert.AreEqual(new Vector2(9, 5), Clusterbyte.SpawnAtTile(18, 10));
            Assert.AreEqual(new Vector2(-9, 5), Clusterbyte.SpawnAtTile(1, 10));
        }

        [Test]
        public void InvalidTileIndexing()
        {
            Assert.Throws<System.Exception>(() => Clusterbyte.SpawnAtTile(19, 2));
            Assert.Throws<System.Exception>(() => Clusterbyte.SpawnAtTile(-1, 1));
            Assert.Throws<System.Exception>(() => Clusterbyte.SpawnAtTile(4, -4));
            Assert.Throws<System.Exception>(() => Clusterbyte.SpawnAtTile(3, 0));
        }
    }
}