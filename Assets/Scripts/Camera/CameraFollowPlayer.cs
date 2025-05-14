using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public Transform Player;
    private Vector3 cameraDistance;

    private void Start()
    {
        cameraDistance = transform.position - Player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Player.transform.position + cameraDistance;
    }
}
