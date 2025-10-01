using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class EnemyAttackRange : MonoBehaviour
{
    private SphereCollider attackRangeCollider;
    private EnemyStats enemyStats;
    private EnemyMovement enemyMovement;

    private GameObject normalAttack;
    private List<GameObject> enemiesInRange = new List<GameObject>();

    private GameObject closestEnemy;

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

    private void Update()
    {
        if (enemiesInRange.Count > 0)
        {
            var closestDistance = float.MaxValue;
            foreach (GameObject enemy in enemiesInRange)
            {
                var distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy;
                }
            }

            enemyMovement.AttackState(closestEnemy);
        }
        else
        {
            enemyMovement.MovementState();
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.parent.CompareTag("Character"))
        {
            enemiesInRange.Add(collider.transform.parent.gameObject);
        }
        if (collider.transform.parent.CompareTag("Payload"))
        {
            print(collider.transform.parent);
            enemiesInRange.Add(collider.transform.parent.gameObject);
        }
    }
    private void OnTriggerExit(Collider collider)
    {
        if (collider.transform.parent.CompareTag("Character"))
        {
            enemiesInRange.Remove(collider.transform.parent.gameObject);
        }
        if (collider.transform.parent.CompareTag("Payload"))
        {
            enemiesInRange.Remove(collider.transform.parent.gameObject);
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
