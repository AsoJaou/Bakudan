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

            if (moveDir !=  Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(moveDir);
            }
        }
    }

    //Methods
    void NormalAttack(GameObject target)
    {
        StopMoving();
        transform.LookAt(target.transform.position);
        GameObject NormalAttackInstance = Instantiate(transform.Find("Normal Attack").gameObject);
        NormalAttackInstance.transform.position = transform.position;
        NormalAttackInstance.transform.LookAt(target.transform);
        NormalAttackInstance.SendMessage("AttackTarget", target, SendMessageOptions.DontRequireReceiver);
    }

    void EnemyCheck(GameObject hitObject)
    {
        if (enemiesInRange.Contains(hitObject))
        {
            NormalAttack(hitObject);
        }
        else
        {
            MoveToAttack(hitObject);
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
        while (!enemiesInRange.Contains(target))
        {
            yield return null;
        }
        StopMoving();
        NormalAttack(target);
    }

    //Raycast
    RaycastHit? MouseLayerDetection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        int DetectableLayers =
            (1 << LayerMask.NameToLayer("Ground")) |
            (1 << LayerMask.NameToLayer("Enemy"));

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, DetectableLayers))
        {
            return hit;
        }
        else
        {
            return null;
        }
    }

    //Find Closest Enemy
    /*
    GameObject FindClosestEnemy()
    {
        closestEnemyDistance = Mathf.Infinity;
        foreach (GameObject enemy in enemiesInRange)
        {
            float distance = Vector3.Distance(hitPosition, enemy.transform.position);

            if (distance < closestEnemyDistance)
            {
                closestEnemyDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }
    */

    //Message Methods
    void EnemyEnterAttackRange(Collider Enemy)
    {
        if (!enemiesInRange.Contains(Enemy.gameObject))
        {
            enemiesInRange.Add(Enemy.gameObject);
        }
    }

    void EnemyExitAttackRange(Collider Enemy)
    {
        if (enemiesInRange.Contains(Enemy.gameObject))
        {
            enemiesInRange.Remove(Enemy.gameObject);
        }
    }
}
