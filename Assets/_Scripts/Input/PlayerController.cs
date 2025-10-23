using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using System.Collections;
using Unity.VisualScripting;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    // Handles mouse and keyboard input bindings.
    private InputAction rightClick;
    private InputAction leftClick;
    private InputAction aKey;

    // Tracks movement targets and owns the NavMesh agent.
    private Coroutine isMoving;
    private Vector3 currentPos;
    private Vector3 targetPos;
    private Vector3 displacement;
    private Vector3 direction;
    private NavMeshAgent agent;

    // Manages attack range checks and chosen enemy.
    private GameObject attackRange;
    private AttackRange attackRangeComponent;
    private GameObject closestEnemy;
    private float closestEnemyDistance;
    private List<GameObject> enemiesInRange = new List<GameObject>();

    private void Awake()
    {
        // Cache components and grab the input actions we listen to.
        agent = GetComponent<NavMeshAgent>();
        attackRange = transform.Find("Attack Range").gameObject;
        if (attackRange != null)
        {
            attackRangeComponent = attackRange.GetComponent<AttackRange>();
        }

        rightClick = InputSystem.actions.FindAction("Right Mouse Button");
        leftClick = InputSystem.actions.FindAction("Left Mouse Button");
        aKey = InputSystem.actions.FindAction("A");
    }

    private void Start()
    {
        // Force the agent to react instantly while we handle rotation ourselves.
        agent.acceleration = 9999f;
        agent.angularSpeed = 0f;
        agent.updateRotation = false;
    }

    private void Update()
    {
        // Keep moving along the agent path and face the travel direction.
        if (agent.hasPath)
        {
            Vector3 moveDir = agent.desiredVelocity.normalized;
            agent.velocity = moveDir * PlayerStats.Instance.Speed * 0.1f;

            if (moveDir != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(moveDir);
            }
        }
    }

    void MoveToPosition(Vector3 target)
    {
        // Walk toward the requested point.
        StopMoving();
        agent.SetDestination(target);
    }

    void MoveToAttack(GameObject target)
    {
        // Close the gap to the selected enemy.
        StopMoving();

        targetPos = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
        isMoving = StartCoroutine(MoveToAttackCorutine(target, targetPos));
    }

    public void StopMoving()
    {
        // Cancel the active path immediately.
        agent.ResetPath();
        agent.velocity = Vector3.zero;
        agent.Warp(transform.position);
    }

    IEnumerator MoveToAttackCorutine(GameObject target, Vector3 targetPos)
    {
        // Chase until the enemy is reported inside attack range, then strike.
        agent.SetDestination(targetPos);
        while (!GameManager.Instance.EnemiesInRange.Contains(target))
        {
            yield return null;
        }
        StopMoving();
        attackRangeComponent?.NormalAttack(target);
    }
}

