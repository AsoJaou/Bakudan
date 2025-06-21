using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollowPlayer : MonoBehaviour
{
    [Header("Public Variables")]
    [SerializeField]
    public Transform Player;

    //Input Actions
    private InputAction Y;
    private InputAction Space;

    //Camera Settings
    private Vector3 cameraDistance;
    private bool cameraFollow;

    //Unlocked Camera
    private float edgeThreshold = 5f;
    private float cameraMoveSpeed = 40f;
    private Vector3 mousePosition;
    private Vector3 cameraPosition;
    private bool isTouchingLeftEdge;
    private bool isTouchingRightEdge;
    private bool isTouchingTopEdge;
    private bool isTouchingBottomEdge;

    private void Awake()
    {
        Y = InputSystem.actions.FindAction("Y");
        Space = InputSystem.actions.FindAction("Space");
        cameraDistance = transform.position - Player.transform.position;
        cameraFollow = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Y.WasPressedThisFrame())
        {
            cameraFollow = !cameraFollow;
        }

        if (cameraFollow)
        {
            LockedCamera();
        }
        else
        {
            if (Space.IsPressed())
            {
                LockedCamera();
            }
            if (IsTouchingEdge())
            {
                UnlockedCameraMovement();
            }
        }
    }

    void LockedCamera()
    {
        transform.position = Player.transform.position + cameraDistance;
    }

    bool IsTouchingEdge()
    {
        mousePosition = Input.mousePosition;
        isTouchingLeftEdge = mousePosition.x <= edgeThreshold;
        isTouchingRightEdge = mousePosition.x >= Screen.width - edgeThreshold;
        isTouchingTopEdge = mousePosition.y >= Screen.height - edgeThreshold;
        isTouchingBottomEdge = mousePosition.y <= edgeThreshold;

        return isTouchingBottomEdge || isTouchingLeftEdge || isTouchingRightEdge || isTouchingTopEdge;
    }

    void UnlockedCameraMovement()
    {
        if (isTouchingLeftEdge)
        {
            cameraPosition = transform.position;
            cameraPosition.x -= cameraMoveSpeed * Time.deltaTime;
            transform.position = cameraPosition;
        }
        if (isTouchingRightEdge)
        {
            cameraPosition = transform.position;
            cameraPosition.x += cameraMoveSpeed * Time.deltaTime;
            transform.position = cameraPosition;
        }
        if (isTouchingTopEdge)
        {
            cameraPosition = transform.position;
            cameraPosition.z += cameraMoveSpeed * Time.deltaTime;
            transform.position = cameraPosition;
        }
        if (isTouchingBottomEdge)
        {
            cameraPosition = transform.position;
            cameraPosition.z -= cameraMoveSpeed * Time.deltaTime;
            transform.position = cameraPosition;
        }
    }
}
