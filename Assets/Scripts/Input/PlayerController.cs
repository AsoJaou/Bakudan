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
    private float baseMoveSpeed;

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

    //Raycast Variables
    private Vector3 hitPosition;
    private GameObject hitObject;

    //Attack Range Variables
    private GameObject attackRange;
    private GameObject closestEnemy;
    private float closestEnemyDistance;
    private List<GameObject> enemiesInRange = new List<GameObject>();

    private void Awake()
    {
        attackRange = transform.Find("Attack Range").gameObject;
        rightClick = InputSystem.actions.FindAction("Right Mouse Button");
        leftClick = InputSystem.actions.FindAction("Left Mouse Button");
        aKey = InputSystem.actions.FindAction("A");
    }

    private void Update()
    {
        RaycastHit? maybeHit = MouseLayerDetection();

        if (maybeHit is RaycastHit hit)
        {
            hitPosition = hit.point;
            hitObject = hit.collider.gameObject;
        }

        if (rightClick.WasPressedThisFrame())
        {
            if (LayerMask.LayerToName(hitObject.layer) == "Ground")
            {
                MoveToPosition();
            }
            else if (LayerMask.LayerToName(hitObject.layer) == "Enemy")
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
        }

        if (aKey.IsPressed())
        {
            attackRange.SendMessage("ShowAttackRange");
            if (leftClick.WasPressedThisFrame())
            {
                if (enemiesInRange.Count > 0f)
                {
                    FindClosestEnemy();
                    NormalAttack(closestEnemy);
                }
                else if (LayerMask.LayerToName(hitObject.layer) == "Enemy")
                {
                    MoveToAttack(hitObject);
                }
                else
                {
                    MoveToPosition();
                }
            }
        }
        else if (aKey.WasReleasedThisFrame())
        {
            attackRange.SendMessage("HideAttackRange");
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

    void MoveToPosition()
    {
        StopMoving();

        targetPos = new Vector3(hitPosition.x, transform.position.y, hitPosition.z);
        isMoving = StartCoroutine(MoveToPositionCorutine(targetPos));
    }

    void MoveToAttack(GameObject target)
    {
        StopMoving();

        targetPos = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
        isMoving = StartCoroutine(MoveToAttackCorutine(target, targetPos));
    }

    void StopMoving()
    {
        if (isMoving != null)
        {
            StopCoroutine(isMoving);
        }
        targetPos = Vector3.zero;
        displacement = Vector3.zero;
        direction = Vector3.zero;
    }

    //Coroutines
    IEnumerator MoveToPositionCorutine(Vector3 targetPos)
    {
        transform.LookAt(targetPos);
        while (Vector3.Distance(transform.position, targetPos) > 0.1f)
        {
            currentPos = transform.position;
            displacement = targetPos - currentPos;
            direction = displacement.normalized * Time.deltaTime;
            transform.position += direction * baseMoveSpeed * 0.1f;

            yield return null;
        }
    }

    IEnumerator MoveToAttackCorutine(GameObject target, Vector3 targetPos)
    {
        while (!enemiesInRange.Contains(target))
        {
            transform.LookAt(targetPos);
            currentPos = transform.position;
            displacement = target.transform.position - currentPos;
            direction = displacement.normalized * Time.deltaTime;
            transform.position += direction * baseMoveSpeed * 0.1f;

            yield return null;
        }
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
    GameObject FindClosestEnemy ()
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
