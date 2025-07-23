using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using System.Collections;
using Unity.VisualScripting;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    [Header("Move Settings")]
    [SerializeField]
    private float baseMoveSpeed = 67f;

    //Input Actions
    private InputAction rightClick;
    private InputAction leftClick;
    private InputAction aKey;

    //Movement Variables
    private Coroutine isMoving;
    private Vector3 currentPos;
    private Vector3 targetPos;
    private Vector3 displacement;
    private Vector3 direction;
    private NavMeshAgent agent;

    //Attack Range Variables
    private GameObject attackRange;
    private GameObject closestEnemy;
    private float closestEnemyDistance;
    private List<GameObject> enemiesInRange = new List<GameObject>();

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        attackRange = transform.Find("Attack Range").gameObject;
        rightClick = InputSystem.actions.FindAction("Right Mouse Button");
        leftClick = InputSystem.actions.FindAction("Left Mouse Button");
        aKey = InputSystem.actions.FindAction("A");
    }

    private void Start()
    {
        agent.acceleration = 9999f;
        agent.angularSpeed = 0f;
        agent.updateRotation = false;
    }

    private void Update()
    {
        // Nav Agent Movement
        if (agent.hasPath)
        {
            Vector3 moveDir = agent.desiredVelocity.normalized;
            agent.velocity = moveDir * baseMoveSpeed * 0.1f;

            if (moveDir != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(moveDir);
            }
        }
    }

    void MoveToPosition(Vector3 target)
    {
        StopMoving();
        agent.SetDestination(target);
    }

    void MoveToAttack(GameObject target)
    {
        StopMoving();

        Debug.Log("Moving to attack: " + target.name);
        targetPos = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
        isMoving = StartCoroutine(MoveToAttackCorutine(target, targetPos));
    }

    void StopMoving()
    {
        agent.ResetPath();
        agent.velocity = Vector3.zero;
        agent.Warp(transform.position);
    }

    IEnumerator MoveToAttackCorutine(GameObject target, Vector3 targetPos)
    {
        agent.SetDestination(targetPos);
        while (!GameManager.Instance.EnemiesInRange.Contains(target))
        {
            yield return null;
        }
        StopMoving();
        attackRange.SendMessage("NormalAttack", target);
    }
}