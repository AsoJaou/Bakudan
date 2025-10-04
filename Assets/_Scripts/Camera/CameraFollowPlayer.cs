using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollowPlayer : MonoBehaviour
{
    [Header("Private Variables")]
    [SerializeField] private Transform player;

    [Header("Clamp Bounds (World Space)")]
    [SerializeField] private Vector2 minBounds = new Vector2(-50f, -50f);
    [SerializeField] private Vector2 maxBounds = new Vector2(50f, 50f);

    [Header("Unlocked Camera Settings")]
    [SerializeField] private float edgeThreshold = 5f;
    [SerializeField] private float cameraMoveSpeed = 60f;

    private InputAction toggleFollowAction;
    private InputAction holdFollowAction;

    private Vector3 cameraDistance;
    private bool cameraFollow = true;

    private Vector3 mousePosition;
    private bool isTouchingLeftEdge;
    private bool isTouchingRightEdge;
    private bool isTouchingTopEdge;
    private bool isTouchingBottomEdge;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        toggleFollowAction = InputSystem.actions.FindAction("Y");
        holdFollowAction = InputSystem.actions.FindAction("Space");
        cameraDistance = transform.position - player.position;
    }

    private void Update()
    {
        if (toggleFollowAction.WasPressedThisFrame())
        {
            cameraFollow = !cameraFollow;
        }

        if (cameraFollow || holdFollowAction.IsPressed())
        {
            LockedCamera();
        }
        else if (IsTouchingEdge())
        {
            UnlockedCameraMovement();
        }

        ClampCameraPosition();
    }

    private void LockedCamera()
    {
        transform.position = player.position + cameraDistance;
    }

    private bool IsTouchingEdge()
    {
        mousePosition = Input.mousePosition;
        isTouchingLeftEdge = mousePosition.x <= edgeThreshold;
        isTouchingRightEdge = mousePosition.x >= Screen.width - edgeThreshold;
        isTouchingTopEdge = mousePosition.y >= Screen.height - edgeThreshold;
        isTouchingBottomEdge = mousePosition.y <= edgeThreshold;

        return isTouchingBottomEdge || isTouchingLeftEdge || isTouchingRightEdge || isTouchingTopEdge;
    }

    private void UnlockedCameraMovement()
    {
        Vector3 position = transform.position;

        if (isTouchingLeftEdge)
        {
            position.x -= cameraMoveSpeed * Time.deltaTime;
        }
        if (isTouchingRightEdge)
        {
            position.x += cameraMoveSpeed * Time.deltaTime;
        }
        if (isTouchingTopEdge)
        {
            position.z += cameraMoveSpeed * Time.deltaTime;
        }
        if (isTouchingBottomEdge)
        {
            position.z -= cameraMoveSpeed * Time.deltaTime;
        }

        transform.position = position;
    }

    private void ClampCameraPosition()
    {
        Vector3 position = transform.position;
        position.x = Mathf.Clamp(position.x, minBounds.x, maxBounds.x);
        position.z = Mathf.Clamp(position.z, minBounds.y, maxBounds.y);
        transform.position = position;
    }
}