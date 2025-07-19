using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    [Header("Base Stats")]
    [SerializeField] private float baseSpeed = 67f;
    [SerializeField] private float baseAttackSpeed = 15f;
    [SerializeField] private float baseAttackDamage = 0f;
    [SerializeField] private float baseHealth = 600f;

    public float BaseSpeed => baseSpeed;
    public float BaseAttackSpeed => baseAttackSpeed;
    public float BaseAttackDamage => baseAttackDamage;
    public float BaseHealth => baseHealth;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}