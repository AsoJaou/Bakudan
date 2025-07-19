public interface IStatModifier
{
    float BonusSpeed { get; }
    float BonusAttackSpeed { get; }
    float BonusAttackDamage { get; }
    float BonusHealth { get; }
}

public class StatModifier : IStatModifier
{
    public float BonusSpeed { get; private set; }
    public float BonusAttackSpeed { get; private set; }
    public float BonusAttackDamage { get; private set; }
    public float BonusHealth { get; private set; }
    public StatModifier(float speed, float attackSpeed, float attackDamage, float health)
    {
        BonusSpeed = speed;
        BonusAttackSpeed = attackSpeed;
        BonusAttackDamage = attackDamage;
        BonusHealth = health;
    }
}