using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AsteroidData", menuName = "Asteroids/New Asteroid Data")]
public class AsteroidData : ScriptableObject
{
    public GameObject asteroidPrefab;
    public int health = 3;
    public int spawnAttempts = 1;

    [System.Serializable]
    public class SpawnRoll
    {
        public AsteroidDataList asteroidDataList;
        public float spawnChance;
    }

    public List<SpawnRoll> spawnRolls;
}
