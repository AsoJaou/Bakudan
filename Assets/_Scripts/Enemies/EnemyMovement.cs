using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private GameObject player;
    private GameObject payload;

    private bool attacking = false;
    private NavMeshAgent agent;
    private EnemyStats enemyStats;
    private EnemyAttackRange enemyAttackRange;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        payload = GameObject.FindGameObjectWithTag("Payload");
        agent = GetComponent<NavMeshAgent>();
        enemyStats = GetComponent<EnemyStats>();
        enemyAttackRange = GetComponentInChildren<EnemyAttackRange>();
    }

    private void Start()
    {
        agent.acceleration = 9999f;
        agent.updateRotation = true;
    }

    private void Update()
    {
        if (!attacking)
        {
            agent.SetDestination(payload.transform.position);
        }
        else if (attacking)
        {
            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));

        }

        if (agent.hasPath)
        {
            Vector3 moveDir = agent.desiredVelocity.normalized;
            agent.velocity = moveDir * enemyStats.speed * 0.1f;

        }
    }

    public void AttackState(GameObject player)
    {
        attacking = true;
        agent.ResetPath();
        agent.velocity = Vector3.zero;
        agent.Warp(transform.position);
    }

    public void MovementState()
    {
        attacking = false;
    }
}
