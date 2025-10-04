using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    [Header("Base Stats")]
    [SerializeField] private float baseSpeed = 67f;
    [SerializeField] private float baseAttackSpeed = 15f;
    [SerializeField] private float baseAttackDamage = 20f;
    [SerializeField] private float baseHealth = 600f;
    [SerializeField] private float baseAttackRange = 70f;
    [SerializeField] private float currentHealth;

    public float Speed => baseSpeed;
    public float AttackSpeed => baseAttackSpeed;
    public float AttackDamage => baseAttackDamage;
    public float Health => baseHealth;
    public float AttackRange => baseAttackRange;
    public float CurrentHealth => currentHealth;

    public GameObject healthBar;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

    }

    private void Start()
    {
        currentHealth = baseHealth;
    }

    public void HealthChange(float amount)
    {
        if (Mathf.Approximately(amount, 0f))
        {
            return;
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0f, baseHealth);
        healthBar.SendMessage("UpdateHealthBar", currentHealth / baseHealth);
    }
}