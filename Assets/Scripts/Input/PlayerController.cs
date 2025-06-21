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

    //Raycast Variables
    private Vector3 hitPosition;
    private GameObject hitObject;

    //Attack Range Variables
    private GameObject attackRange;
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
        else
        {
            Debug.Log("Invalid Position");
        }

        if (rightClick.WasPressedThisFrame())
        {
            if (LayerMask.LayerToName(hitObject.layer) == "Ground")
            {
                Move();
            }
            else if (LayerMask.LayerToName(hitObject.layer) == "Enemy")
            {
                transform.LookAt(hitObject.transform.position);
                if (isMoving != null)
                {
                    StopCoroutine(isMoving);
                }
                if (enemiesInRange.Contains(hitObject))
                {
                    NormalAttack(hitObject);
                }
                else
                {
                    isMoving = StartCoroutine(MoveToAttack(hitObject));
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
                    Debug.Log("Enemy Hit");
                }
                else
                {
                    Move();
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
        GameObject NormalAttackInstance = Instantiate(transform.Find("Normal Attack").gameObject);
        NormalAttackInstance.transform.position = transform.position;
        NormalAttackInstance.transform.LookAt(target.transform);
        NormalAttackInstance.SendMessage("AttackTarget", target, SendMessageOptions.DontRequireReceiver);
    }

    void Move ()
    {
        if (isMoving != null)
        {
            StopCoroutine(isMoving);
        }
        targetPos = Vector3.zero;
        displacement = Vector3.zero;
        direction = Vector3.zero;

        targetPos = new Vector3(hitPosition.x, transform.position.y, hitPosition.z);
        transform.LookAt(targetPos);
        isMoving = StartCoroutine(MoveToPosition(targetPos));
    }

    //Coroutines
    IEnumerator MoveToPosition(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.1f)
        {
            currentPos = transform.position;
            displacement = target - currentPos;
            direction = displacement.normalized * Time.deltaTime;
            transform.position += direction * baseMoveSpeed * 0.1f;

            yield return null;
        }
    }

    IEnumerator MoveToAttack(GameObject target)
    {
        while (!enemiesInRange.Contains(target))
        {
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
