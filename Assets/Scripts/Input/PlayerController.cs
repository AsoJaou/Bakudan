using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public GameObject MovableTerrain;

    private InputAction clickToMove;

    //Movement Variables
    private Coroutine isMoving;
    private Vector3 currentPos;
    private Vector3 targetPos;
    private Vector3 displacement;
    private Vector3 direction;

    [Header("Move Settings")]
    [SerializeField]
    private float moveSpeed = 10f;

    private void Start()
    {
        clickToMove = InputSystem.actions.FindAction("Move");
    }

    private void Update()
    {
        if (clickToMove.WasPressedThisFrame())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 HitPosition = hit.point;
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
            }
            else
            {
                Debug.Log("Unable to Move to this location");
            }
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
}
