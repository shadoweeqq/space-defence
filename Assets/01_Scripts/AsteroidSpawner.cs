using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public AsteroidDataList asteroidDataList; // List of asteroid types
    public Vector2 chunkSize = new Vector2(20f, 20f); // Size of each "chunk"
    public int asteroidsPerChunk = 5;
    public float spawnRadius = 50f; // How far away new asteroids can generate

    private Dictionary<Vector2Int, bool> visitedChunks = new Dictionary<Vector2Int, bool>(); // Stores visited chunks
    private List<GameObject> spawnedAsteroids = new List<GameObject>(); // Keeps track of existing asteroids

    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        GenerateAsteroidsAroundPlayer();
    }

    private void Update()
    {
        GenerateAsteroidsAroundPlayer();
    }

    private void GenerateAsteroidsAroundPlayer()
    {
        Vector2Int currentChunk = GetChunk(player.position);

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector2Int chunkToCheck = new Vector2Int(currentChunk.x + x, currentChunk.y + y);

                if (!visitedChunks.ContainsKey(chunkToCheck))
                {
                    visitedChunks[chunkToCheck] = true;
                    SpawnAsteroidsInChunk(chunkToCheck);
                }
            }
        }
    }

    private void SpawnAsteroidsInChunk(Vector2Int chunk)
    {
        Vector2 chunkCenter = GetChunkCenter(chunk);

        for (int i = 0; i < asteroidsPerChunk; i++)
        {
            AsteroidData asteroidData = GetRandomAsteroidData();
            if (asteroidData == null) continue; // Skip if no valid data

            Vector2 spawnPos = chunkCenter + new Vector2(
                Random.Range(-chunkSize.x / 2, chunkSize.x / 2),
                Random.Range(-chunkSize.y / 2, chunkSize.y / 2)
            );

            GameObject newAsteroid = Instantiate(asteroidData.asteroidPrefab, spawnPos, Quaternion.identity);
            spawnedAsteroids.Add(newAsteroid);
        }
    }

    private AsteroidData GetRandomAsteroidData()
    {
        if (asteroidDataList == null || asteroidDataList.asteroids.Count == 0) return null;
        return asteroidDataList.asteroids[Random.Range(0, asteroidDataList.asteroids.Count)];
    }

    private Vector2Int GetChunk(Vector2 position)
    {
        return new Vector2Int(Mathf.FloorToInt(position.x / chunkSize.x), Mathf.FloorToInt(position.y / chunkSize.y));
    }

    private Vector2 GetChunkCenter(Vector2Int chunk)
    {
        return new Vector2(chunk.x * chunkSize.x + chunkSize.x / 2, chunk.y * chunkSize.y + chunkSize.y / 2);
    }
}
