using UnityEngine;
using UnityEngine.UI;

public class SimpleTurret : MonoBehaviour, IDamageable
{
    public float range = 10f;
    public float fireRate = 1f;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float aimVariation = 5f; // Maximum angle variation in degrees

    private float lastShotTime;
    private Transform target;

    [Header("Health")]
    public float currentHealth = 100f;
    public float maxHealth = 100f;
    public float regenRate = 0f;
    public Slider healthBar;

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    void UpdateHealthBar()
    {
        if (currentHealth < maxHealth) currentHealth += regenRate * Time.deltaTime;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }

    private void Update()
    {
        FindTarget();
        if (target != null)
        {
            RotateTowardsTarget();
            Attack();
        }
        UpdateHealthBar();
    }

    private void FindTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float closestDistance = range;
        Transform closestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy.transform;
            }
        }

        target = closestEnemy;
    }

    private void RotateTowardsTarget()
    {
        if (target == null) return;

        Vector2 predictedPosition = PredictTargetPosition();
        Vector2 direction = (predictedPosition - (Vector2)firePoint.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        // Apply random variation to make it less accurate
        angle += Random.Range(-aimVariation, aimVariation);

        firePoint.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void Attack()
    {
        if (Time.time - lastShotTime >= fireRate && target != null)
        {
            lastShotTime = Time.time;
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }

    private Vector2 PredictTargetPosition()
    {
        if (target == null) return firePoint.position;

        Rigidbody2D enemyRb = target.GetComponent<Rigidbody2D>();
        Vector2 enemyPos = target.position;
        Vector2 enemyVelocity = enemyRb ? enemyRb.linearVelocity : Vector2.zero;
        float bulletSpeed = bulletPrefab.GetComponent<Bullet>().speed;

        float distance = Vector2.Distance(transform.position, enemyPos);
        float timeToImpact = distance / bulletSpeed;

        return enemyPos + (enemyVelocity * timeToImpact);
    }
}
