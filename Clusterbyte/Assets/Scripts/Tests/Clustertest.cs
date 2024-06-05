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
            Assert.AreEqual(-9, Clusterbyte.WorldToGrid(1, 1).x);
            Assert.AreEqual(9, Clusterbyte.WorldToGrid(18, 1).x);
            Assert.AreEqual(9, Clusterbyte.WorldToGrid(18, 10).x);
            Assert.AreEqual(-9, Clusterbyte.WorldToGrid(1, 10).x);
            // y
            Assert.AreEqual(-5, Clusterbyte.WorldToGrid(1, 1).y);
            Assert.AreEqual(-5, Clusterbyte.WorldToGrid(18, 1).y);
            Assert.AreEqual(5, Clusterbyte.WorldToGrid(18, 10).y);
            Assert.AreEqual(5, Clusterbyte.WorldToGrid(1, 10).y);
            // Full corner vector tests
            Assert.AreEqual(new Vector2(-9, -5), Clusterbyte.WorldToGrid(1, 1));
            Assert.AreEqual(new Vector2(9, -5), Clusterbyte.WorldToGrid(18, 1));
            Assert.AreEqual(new Vector2(9, 5), Clusterbyte.WorldToGrid(18, 10));
            Assert.AreEqual(new Vector2(-9, 5), Clusterbyte.WorldToGrid(1, 10));
        }

        [Test]
        public void WorldTileIndexing()
        {
            // Reverse operation of GetGridCoordinate

            // Individual corner x
            Assert.AreEqual(1, Clusterbyte.GridToWorld(-9, -5).x);
            Assert.AreEqual(18, Clusterbyte.GridToWorld(9, -5).x);
            Assert.AreEqual(18, Clusterbyte.GridToWorld(9, 5).x);
            Assert.AreEqual(1, Clusterbyte.GridToWorld(-9, 5).x);
            // y
            Assert.AreEqual(1, Clusterbyte.GridToWorld(-9, -5).y);
            Assert.AreEqual(1, Clusterbyte.GridToWorld(9, -5).y);
            Assert.AreEqual(10, Clusterbyte.GridToWorld(9, 5).y);
            Assert.AreEqual(10, Clusterbyte.GridToWorld(-9, 5).y);
            // Full corner vector tests
            Assert.AreEqual(new Vector2(1, 1), Clusterbyte.GridToWorld(-9, -5));
            Assert.AreEqual(new Vector2(18, 1), Clusterbyte.GridToWorld(9, -5));
            Assert.AreEqual(new Vector2(18, 10), Clusterbyte.GridToWorld(9, 5));
            Assert.AreEqual(new Vector2(1, 10), Clusterbyte.GridToWorld(-9, 5));
        }

        [Test]
        public void TerrainOccupationIndexing()
        {
            Clusterbyte.ResetTerrainOccupiedCoordinates();

            // Valid cases
            Clusterbyte.SpawnAtTile(1, 1, new GameObject(), true);
            Clusterbyte.SpawnAtTile(18, 10, new GameObject(), true);
            Assert.IsTrue(Clusterbyte.IsTileOccupiedByTerrain(1, 1));
            Assert.IsTrue(Clusterbyte.IsTileOccupiedByTerrain(18, 10));

            // Invalid cases
            Clusterbyte.SpawnAtTile(4, 5, new GameObject());
            Clusterbyte.SpawnAtTile(12, 1, new GameObject());
            Assert.IsFalse(Clusterbyte.IsTileOccupiedByTerrain(4, 5));
            Assert.IsFalse(Clusterbyte.IsTileOccupiedByTerrain(12, 1));

            // Invalid range testing
            Assert.IsFalse(Clusterbyte.IsTileOccupiedByTerrain(23, 4));
            Assert.IsFalse(Clusterbyte.IsTileOccupiedByTerrain(2, 34));
            Assert.IsFalse(Clusterbyte.IsTileOccupiedByTerrain(62, 354));
        }

        [Test]
        public void InvalidTileIndexing()
        {
            Assert.Throws<System.Exception>(() => Clusterbyte.WorldToGrid(19, 2));
            Assert.Throws<System.Exception>(() => Clusterbyte.WorldToGrid(-1, 1));
            Assert.Throws<System.Exception>(() => Clusterbyte.WorldToGrid(4, -4));
            Assert.Throws<System.Exception>(() => Clusterbyte.WorldToGrid(3, 0));
        }
    }
}