using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    public float Speed = 67f;
    public float NormalAttackSpeed = 15f;
    public float NormalAttackDamage;
    public float Health = 600f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void BonusSpeed(float speed)
    {
        Speed += speed;
    }

    public void BonusAttackSpeed(float normalAttackSpeed)
    {
        NormalAttackSpeed += normalAttackSpeed;
    }

    public void BonusNormalAttackDamage(float normalAttackDamage)
    {
        NormalAttackDamage += normalAttackDamage;
    }

    public void BonusHealth(float health)
    {
        Health += health;
    }
}
