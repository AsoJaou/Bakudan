using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollowPlayer : MonoBehaviour
{
    public Transform Player;
    private Vector3 cameraDistance;
    private bool cameraFollow;

    private InputAction Y;

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
            FollowPlayer();
        }
        else
        {
            FollowMouse();
        }
    }

    void FollowPlayer ()
    {
        transform.position = Player.transform.position + cameraDistance;
    }

    void FollowMouse ()
    {
        return;
    }
}
