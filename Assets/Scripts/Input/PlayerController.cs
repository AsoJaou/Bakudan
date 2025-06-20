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
    private float moveSpeed = 10f;

    [Header("Public Variables")]
    [SerializeField]
    //None at the moment

    //Input Actions
    private InputAction rightClick;

    //Movement Variables
    private Coroutine isMoving;
    private Vector3 currentPos;
    private Vector3 targetPos;
    private Vector3 displacement;
    private Vector3 direction;

    //Raycast Variables
    private Vector3 hitPosition;
    private GameObject hitObject;

    //Attack Range Variabels
    private List<GameObject> enemiesInRange = new List<GameObject>();

    private void Start()
    {
        rightClick = InputSystem.actions.FindAction("Right Mouse Button");
    }

    private void Update()
    {
        RaycastHit? maybeHit = MouseLayerDetection();

        if (maybeHit is RaycastHit hit)
        {
            hitPosition = hit.point;
            hitObject = hit.collider.gameObject;

            if (rightClick.WasPressedThisFrame())
            {
                if (LayerMask.LayerToName(hitObject.layer) == "Ground")
                {
                    transform.LookAt(hitPosition);
                    if (isMoving != null)
                    {
                        StopCoroutine(isMoving);
                    }
                    targetPos = Vector3.zero;
                    displacement = Vector3.zero;
                    direction = Vector3.zero;

                    targetPos = new Vector3(hitPosition.x, transform.position.y, hitPosition.z);
                    isMoving = StartCoroutine(MoveToPosition());
                }
                else if (LayerMask.LayerToName(hitObject.layer) == "Enemy")
                {
                    if (enemiesInRange.Contains(hitObject))
                    {
                        StopCoroutine(isMoving);
                        transform.LookAt(hitObject.transform.position);
                        GameObject NormalAttackInstance = Instantiate(transform.Find("Normal Attack").gameObject);
                        NormalAttackInstance.transform.position = transform.position;
                        NormalAttackInstance.transform.LookAt(hitObject.transform);
                        NormalAttackInstance.SendMessage("AttackTarget", hitObject, SendMessageOptions.DontRequireReceiver);
                    }
                    else
                    {
                        Debug.Log("Enemy out of range");
                    }
                }
            }
        }
        else
        {
            Debug.Log("Invalid Position");
        }
    }

    IEnumerator MoveToPosition()
    {
        while (Vector3.Distance(transform.position, targetPos) > 0.1f)
        {
            currentPos = transform.position;
            displacement = targetPos - currentPos;
            direction = displacement.normalized * Time.deltaTime;
            transform.position += direction * moveSpeed;

            yield return null;
        }
    }

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
