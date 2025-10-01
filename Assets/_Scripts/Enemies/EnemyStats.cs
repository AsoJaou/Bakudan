using UnityEngine;

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
        healthBar = transform.Find("Healthbar Canvas").gameObject;
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    void HealthChange(float health)
    {
        currentHealth = Mathf.Clamp(currentHealth + health, 0, maxHealth);

        healthBar.SendMessage("UpdateHealthbar", currentHealth / maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (GameManager.Instance.EnemiesInRange.Contains(gameObject))
        {
            GameManager.Instance.RemoveEnemyFromRange(gameObject);
        }
        Destroy(gameObject);
    }
}
