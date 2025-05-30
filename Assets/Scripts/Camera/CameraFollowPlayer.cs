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
            UnlockedCamera();
        }
    }

    void LockedCamera()
    {
        transform.position = Player.transform.position + cameraDistance;
    }

    void UnlockedCamera()
    {
        mousePosition = Input.mousePosition;
        //private bool isTouchingLeftEdge = Mouse.position.x <= edgeThreshold;
        
    }
}
