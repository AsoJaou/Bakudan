using UnityEngine;
using UnityEngine.TextCore.Text;

public class EnemyAttackRange : MonoBehaviour
{
    private SphereCollider attackRangeCollider;
    private EnemyStats enemyStats;
    private EnemyMovement enemyMovement;

    private GameObject normalAttack;

    private void Awake()
    {
        normalAttack = transform.parent.Find("Normal Attack").gameObject;
        enemyMovement = GetComponentInParent<EnemyMovement>();
        enemyStats = GetComponentInParent<EnemyStats>();
        attackRangeCollider = GetComponent<SphereCollider>();
    }

    private void Start()
    {
        attackRangeCollider.radius = enemyStats.attackRange * 0.1f;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.parent.CompareTag("Character"))
        {
            enemyMovement.AttackState(collider.transform.parent.gameObject);
        }
    }
    private void OnTriggerExit(Collider collider)
    {
        if (collider.transform.parent.CompareTag("Character"))
        {
            enemyMovement.MovementState();
        }
    }
    
    public void Attack(GameObject player)
    {
        GameObject NormalAttackInstance = Instantiate(normalAttack);
        NormalAttackInstance.transform.position = transform.position;
        NormalAttackInstance.transform.LookAt(player.transform);
        NormalAttackInstance.SendMessage("AttackTarget", player, SendMessageOptions.DontRequireReceiver);
    }
}
