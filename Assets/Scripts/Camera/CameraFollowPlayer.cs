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
    private float cameraMoveSpeed = 20f;
    private Vector3 mousePosition;
    private Vector3 displacement;
    private Vector3 direction;

    private void Start()
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
        bool isTouchingLeftEdge = mousePosition.x <= edgeThreshold;
        bool isTouchingRightEdge = mousePosition.x >= Screen.width - edgeThreshold;
        bool isTouchingTopEdge = mousePosition.y >= Screen.height - edgeThreshold;
        bool isTouchingBottomEdge = mousePosition.y <= edgeThreshold;

        return isTouchingBottomEdge || isTouchingLeftEdge || isTouchingRightEdge || isTouchingTopEdge;
    }

    void UnlockedCameraMovement()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        int GroundLayerMask = 1 << LayerMask.NameToLayer("Ground");

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, GroundLayerMask))
        {
            Vector3 HitPosition = hit.point;
            displacement = new Vector3(HitPosition.x - transform.position.x, 0, HitPosition.z - transform.position.z);
            direction = displacement.normalized * Time.deltaTime;
            transform.position += direction * cameraMoveSpeed;
        }
    }
}
