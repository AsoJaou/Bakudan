using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Move Settings")]
    [SerializeField]
    private float moveSpeed = 10f;

    [Header("Public Variables")]
    [SerializeField]
    public GameObject MovableTerrain;

    //Input Actions
    private InputAction rightClick;

    //Movement Variables
    private Coroutine isMoving;
    private Vector3 currentPos;
    private Vector3 targetPos;
    private Vector3 displacement;
    private Vector3 direction;

    private void Start()
    {
        rightClick = InputSystem.actions.FindAction("Right Mouse Button");
    }

    private void Update()
    {
        RaycastHit? Hit = MouseLayerDetection();

        Debug.Log(Hit);
        /*
        if (HitTarget.name == "Ground")
        {
            Debug.Log("Ground");
        }
        if (HitTarget.name == "CollisionBody")
        {
            Debug.Log(HitTarget.transform.parent?.gameObject);
        }
        */
        if (rightClick.WasPressedThisFrame())
        {
            /*
            if (hit.collider.gameObject == MovableTerrain)
            {
                transform.LookAt(HitPosition);
                if (isMoving != null)
                {
                    StopCoroutine(isMoving);
                }
                targetPos = Vector3.zero;
                displacement = Vector3.zero;
                direction = Vector3.zero;

                targetPos = new Vector3(HitPosition.x, transform.position.y, HitPosition.z);
                isMoving = StartCoroutine(MoveToPosition());
            }
            */
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
            //GameObject HitObject = hit.collider.gameObject;
            //GameObject HitObject = hit.collider.gameObject.transform.parent?.gameObject;
            return hit;
        }
        else
        {
            return null;
        }
    }
}
