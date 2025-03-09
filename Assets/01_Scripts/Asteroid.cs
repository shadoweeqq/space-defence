using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour, IDamageable
{
    public AsteroidData asteroidData; // Assign via Inspector

    private PolygonCollider2D polyCollider;
    private Rigidbody2D rb;

    private float health;

    [SerializeField] private AudioQuerySet explosionQuerySet;
    [SerializeField] private ParticleSystem explosionParticle;

    private bool isVisible = true; // Track visibility

    void Awake()
    {
        transform.Rotate(0.0f, 0.0f, Random.Range(0.0f, 360.0f));
        polyCollider = GetComponent<PolygonCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        if (asteroidData == null)
        {
            Debug.LogWarning($"AsteroidData is missing on {gameObject.name}");
            return;
        }

        float rotationSpeed = Random.Range(10f, 50f) * (Random.value > 0.5f ? 1 : -1);
        rb.angularVelocity = rotationSpeed;
        health = asteroidData.health;
    }

    public void TakeDamage(int damage)
    {
        health -= damage * UpgradeManager.Instance.asteroidDamageMultiplier;
        if (health <= 0)
        {
            Explode();
        }
    }

    private void Explode()
    {
        AudioManager.Instance.PlayRandomAudioQuery(explosionQuerySet, transform);
        Instantiate(explosionParticle, transform.position, Quaternion.identity);
        SpawnNewAsteroids();
        //AsteroidSpawner.Instance.ReturnAsteroidToPool(gameObject);
        Destroy(gameObject);
    }

    void SpawnNewAsteroids()
    {
        if (asteroidData == null || asteroidData.spawnRolls.Count == 0) return;

        for (int i = 0; i < asteroidData.spawnAttempts; i++)
        {
            foreach (var roll in asteroidData.spawnRolls)
            {
                if (Random.value <= roll.spawnChance && roll.asteroidDataList != null)
                {
                    AsteroidData spawnData = GetRandomAsteroidFromList(roll.asteroidDataList);
                    if (spawnData != null)
                    {
                        Vector2 spawnPos = GetRandomPointInsideCollider();
                        Instantiate(spawnData.asteroidPrefab, spawnPos, Quaternion.identity);
                    }
                }
            }
        }
    }

    AsteroidData GetRandomAsteroidFromList(AsteroidDataList dataList)
    {
        if (dataList.asteroids.Count == 0) return null;
        return dataList.asteroids[Random.Range(0, dataList.asteroids.Count)];
    }

    Vector2 GetRandomPointInsideCollider()
    {
        if (polyCollider == null) return transform.position;

        Bounds bounds = polyCollider.bounds;
        Vector2 randomPoint = transform.position;
        int attempts = 10;

        while (attempts-- > 0)
        {
            randomPoint = new Vector2(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y)
            );

            if (polyCollider.OverlapPoint(randomPoint))
                return randomPoint;
        }

        return randomPoint; // Fallback
    }

    private void OnBecameVisible()
    {
        if (!isVisible)
        {
            rb.simulated = true;
            isVisible = true;
        }
    }

    private void OnBecameInvisible()
    {
        if (isVisible)
        {
            rb.simulated = false;
            isVisible = false;
        }
        //AsteroidSpawner.Instance.ReturnAsteroidToPool(gameObject);
    }
}
