using UnityEngine;

// Tracks core stats and death logic for enemy units.
public class EnemyStats : MonoBehaviour
{
    public string enemyName;
    public float maxHealth;
    public float damage;
    public float speed;
    public float attackRange;
    public Vector3 enemyPosition;

    private float currentHealth;

    private GameObject healthBar;

    private void Awake()
    {
        // Grab the UI element that displays this enemy's health.
        healthBar = transform.Find("Healthbar Canvas").gameObject;
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    void HealthChange(float health)
    {
        // Apply incoming healing or damage and update the bar.
        currentHealth = Mathf.Clamp(currentHealth + health, 0, maxHealth);

        healthBar.SendMessage("UpdateHealthbar", currentHealth / maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Clean up references and remove the enemy from the scene.
        if (GameManager.Instance.EnemiesInRange.Contains(gameObject))
        {
            GameManager.Instance.RemoveEnemyFromRange(gameObject);
        }
        Destroy(gameObject);
    }
}
