using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Attack")]
    public float health;
    public float fireRate = 1f;
    [Header("Movement")]
    public float speed = 3f;
    public float rotationSpeed = 2f;
    [Header("Detection")]
    public float detectionRange = 10f;
    public float attackRange = 5f;
    public float distanceFromPlayer = 3f;
    public float stationAttackRange = 25f;
    public float obstacleAvoidanceStrength = 2f;
    public float separationDistance = 2f;
    public float separationForce = 1.5f;
    [Header("References")]
    public Transform station;
    public Transform player;
    public LayerMask obstacleMask;
    public LayerMask enemyMask;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public Rigidbody2D rb;
    [Header("Other")]
    public IEnemyState currentState;
    private float lastShotTime;
    [SerializeField] private AudioQuerySet deathQuerySet;
    [SerializeField] private ParticleSystem deathParticle;
    public string currState;



    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        station = GameObject.FindWithTag("Station").transform;
        player = GameObject.FindWithTag("Player").transform;
        ChangeState(new MovingToStationState());
    }

    private void Update()
    {
        currentState.UpdateState(this);
        currState = currentState.ToString();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        AudioManager.Instance.PlayRandomAudioQuery(deathQuerySet, transform);
        Instantiate(deathParticle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (GameObject.FindGameObjectWithTag("WaveSpawner") != null)
        {
            GameObject.FindGameObjectWithTag("WaveSpawner").GetComponent<WaveSpawner>().spawnedEnemies.Remove(gameObject);
        }
    }

    public void ChangeState(IEnemyState newState)
    {
        currentState = newState;
        currentState.EnterState(this);
    }

    public void MoveTowards(Vector2 target)
    {
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        direction += AvoidObstacles(); // Adjust direction to avoid obstacles
        direction.Normalize();

        rb.linearVelocity = direction * speed;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = Mathf.LerpAngle(rb.rotation, angle, rotationSpeed * Time.deltaTime);
    }

    public void Attack(Transform target)
    {
        if (Time.time - lastShotTime >= fireRate)
        {
            lastShotTime = Time.time;
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }

    private Vector2 AvoidObstacles()
    {
        Vector2[] directions = {
            transform.right,                         // Center
            Quaternion.Euler(0, 0, 30) * transform.right,  // Right
            Quaternion.Euler(0, 0, -30) * transform.right  // Left
        };

        foreach (var dir in directions)
        {
            if (Physics2D.Raycast(transform.position, dir, 2f, obstacleMask))
            {
                return Vector2.Perpendicular(dir) * obstacleAvoidanceStrength; // Steer away
            }
        }
        return Vector2.zero;
    }

    private Vector2 AvoidOtherEnemies()
    {
        Collider2D[] nearbyEnemies = Physics2D.OverlapCircleAll(transform.position, separationDistance, enemyMask);
        Vector2 separationForceVec = Vector2.zero;

        foreach (Collider2D enemy in nearbyEnemies)
        {
            if (enemy.transform != transform) // Avoid self
            {
                Vector2 awayFromEnemy = (Vector2)(transform.position - enemy.transform.position);
                separationForceVec += awayFromEnemy.normalized * separationForce;
            }
        }

        return separationForceVec;
    }

    public Transform FindClosestTurret()
    {
        GameObject[] turrets = GameObject.FindGameObjectsWithTag("Turret");
        Transform closest = null;
        float closestDistance = detectionRange;

        foreach (GameObject turret in turrets)
        {
            float distance = Vector2.Distance(transform.position, turret.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = turret.transform;
            }
        }
        return closest;
    }
}