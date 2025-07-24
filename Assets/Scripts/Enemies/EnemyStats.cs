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
}
