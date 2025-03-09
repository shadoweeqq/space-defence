using UnityEngine;
using UnityEngine.UI;

public class Station : MonoBehaviour, IDamageable
{
    [Header("Health")]
    public float currentHealth = 100f;
    public float maxHealth = 100f;
    public float regenRate = 0.1f;
    public Slider healthBar;

    private void Update()
    {
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

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth >= maxHealth) currentHealth = maxHealth;
    }

    private void Die()
    {
        GameManager.Instance.stationDeathScreen.SetActive(true);
    }

    void UpdateHealthBar()
    {
        //if (currentHealth < maxHealth) currentHealth += regenRate * Time.deltaTime;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }
}
