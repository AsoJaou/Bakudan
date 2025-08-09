using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public string enemyName;
    public float maxHealth;
    public float damage;
    public float moveSpeed;
    public float attackRange;

    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void HealthChange(float health)
    {
        Debug.Log("Health Change: " + health);
        currentHealth = Mathf.Clamp(currentHealth + health, 0, maxHealth);
        Debug.Log(Equals(enemyName, "Player") ? "Player Health: " + currentHealth : enemyName + " Health: " + currentHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
