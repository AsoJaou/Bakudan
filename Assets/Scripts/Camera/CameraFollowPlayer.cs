using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollowPlayer : MonoBehaviour
{
    [Header("Public Variables")]
    [SerializeField]
    public Transform Player;

    //Input Actions
    private InputAction Y;

    //Camera Settings
    private Vector3 cameraDistance;
    private bool cameraFollow;

    //Unlocked Camera
    private float edgeThreshold = 5f;
    private Vector3 mousePosition;

    private void Start()
    {
        Y = InputSystem.actions.FindAction("Toggle Camera");
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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            int GroundLayerMask = 1 << LayerMask.NameToLayer("Ground");

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, GroundLayerMask))
            {
                Vector3 HitPosition = hit.point;
                Debug.Log(HitPosition);
            }
        }
    }

    void LockedCamera()
    {
        transform.position = Player.transform.position + cameraDistance;
    }

    bool UnlockedCamera()
    {
        mousePosition = Input.mousePosition;
        bool isTouchingLeftEdge = mousePosition.x <= edgeThreshold;
        bool isTouchingRightEdge = mousePosition.x >= Screen.width - edgeThreshold;
        bool isTouchingTopEdge = mousePosition.y >= Screen.height - edgeThreshold;
        bool isTouchingBottomEdge = mousePosition.y <= edgeThreshold;

        return isTouchingBottomEdge || isTouchingLeftEdge || isTouchingRightEdge || isTouchingTopEdge;
    }
}
