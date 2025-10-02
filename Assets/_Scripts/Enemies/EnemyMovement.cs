using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private GameObject player;
    private GameObject payload;

    private NavMeshAgent agent;
    private EnemyStats enemyStats;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        payload = GameObject.FindGameObjectWithTag("Payload");
        agent = GetComponent<NavMeshAgent>();
        enemyStats = GetComponent<EnemyStats>();
    }

    private void Start()
    {
        agent.acceleration = 9999f;
        agent.updateRotation = true;
    }

    private void Update()
    {
        var distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        var distanceToPayload = Vector3.Distance(transform.position, payload.transform.position);

        if (distanceToPayload < distanceToPlayer)
        {
            agent.SetDestination(payload.transform.position);
        }
        else
        {
            agent.SetDestination(player.transform.position);
        }

        if (agent.hasPath)
        {
            Vector3 moveDir = agent.desiredVelocity.normalized;
            agent.velocity = moveDir * enemyStats.speed * 0.1f;

        }
    }
}
