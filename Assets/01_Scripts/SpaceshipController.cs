using UnityEngine;
using UnityEngine.UI;

public class SpaceshipController : MonoBehaviour, IDamageable
{
    [Header("Movement")]
    public float thrustPower = 5f;
    public float reversePower = 2.5f;
    public float rotationTorque = 5f;
    public float dragFactor = 0.99f;
    public float rotationDrag = 0.95f;

    [Header("Health")]
    public float currentHealth = 100f;
    public float maxHealth = 100f;
    public float regenRate = 0.1f;
    public Slider healthBar;

    [Header("Dash Settings")]
    public float dashForce = 10f;
    public float dashCooldown = 1f;
    private float lastDashTime = 0f;

    [Header("Shooting Settings")]
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float fireRate = 0.2f;
    private float lastShotTime = 0f;

    private Rigidbody2D rb;
    private DraggingObjects draggingObjects;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        draggingObjects = GetComponent<DraggingObjects>();
        rb.linearDamping = 0;
        rb.angularDamping = 0;
    }

    void Update()
    {
        HandleMovement();
        HandleDash();
        HandleShooting();
        UpdateHealthBar();
    }

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
        GameManager.Instance.playerDeathScreen.SetActive(true);
    }

    void UpdateHealthBar()
    {
        if (currentHealth < maxHealth) currentHealth += regenRate * Time.deltaTime;
        if (healthBar == null) return;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }

    void HandleMovement()
    {
        float rotationInput = -Input.GetAxis("Horizontal");
        rb.AddTorque(rotationInput * rotationTorque);
        rb.angularVelocity *= rotationDrag;

        float modifiedThrust = thrustPower * (draggingObjects?.GetSpeedModifier() ?? 1f);

        if (Input.GetKey(KeyCode.W))
        {
            rb.AddForce(transform.up * modifiedThrust);
        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(-transform.up * modifiedThrust);
        }

        rb.linearVelocity *= dragFactor;
    }

    void HandleDash()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > lastDashTime + dashCooldown)
        {
            rb.AddForce(transform.up * dashForce, ForceMode2D.Impulse);
            lastDashTime = Time.time;
        }
    }

    void HandleShooting()
    {
        if (Input.GetKey(KeyCode.Mouse0) && Time.time > lastShotTime + fireRate)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 shootDirection = (mousePosition - bulletSpawnPoint.position).normalized;

            float angle = Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg - 90f;
            Quaternion bulletRotation = Quaternion.Euler(0, 0, angle);

            Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletRotation);
            lastShotTime = Time.time;
        }
    }
}
