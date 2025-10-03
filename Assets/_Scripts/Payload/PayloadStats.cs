using UnityEngine;

public class PayloadStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    [Header("Base Stats")]
    [SerializeField] private float baseSpeed = 67f;
    [SerializeField] private float maxHealth = 1000f;
    [SerializeField] private float currentHealth;

    public float Speed => baseSpeed;
    public float Health => maxHealth;
    public float CurrentHealth => currentHealth;

    private GameObject healthBar;

    private void Awake()
    {
        healthBar = transform.Find("Healthbar Canvas").gameObject;
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void HealthChange(float health)
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
        Destroy(gameObject);
    }
}
