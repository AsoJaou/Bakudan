using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    //Input Actions
    private InputAction rightClick;
    private InputAction leftClick;
    private InputAction aKey;

    //Input Receivers
    private GameObject player;
    private GameObject attackRange;

    //Raycast Variables
    private Vector3 hitPosition;
    private GameObject hitObject;

    

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        attackRange = player.transform.Find("Attack Range").gameObject;

        rightClick = InputSystem.actions.FindAction("Right Mouse Button");
        leftClick = InputSystem.actions.FindAction("Left Mouse Button");
        aKey = InputSystem.actions.FindAction("A");
    }

    private void Update()
    {
        RaycastHit? maybeHit = MouseLayerDetection();

        if (maybeHit is RaycastHit hit)
        {
            hitPosition = hit.point;
            hitObject = hit.collider.gameObject;
        }

        //Right Click
        if (rightClick.WasPressedThisFrame())
        {
            if (LayerMask.LayerToName(hitObject.layer) == "Ground")
            {
                player.SendMessage("MoveToPosition", hitPosition);
            }
            else if (LayerMask.LayerToName(hitObject.layer) == "Enemy")
            {
                attackRange.SendMessage("NormalAttack", hitObject.transform.parent.gameObject);
            }
        }

        //A Key Input
        if (aKey.IsPressed())
        {
            attackRange.SendMessage("ShowAttackRange");
            if (leftClick.WasPressedThisFrame())
            {
                if (LayerMask.LayerToName(hitObject.layer) == "Enemy")
                {
                    if (GameManager.Instance.EnemiesInRange.Contains(hitObject.transform.parent.gameObject))
                    {
                        attackRange.SendMessage("NormalAttack", hitObject.transform.parent.gameObject);
                    }
                    else
                    {
                        player.SendMessage("MoveToAttack", hitObject.transform.parent.gameObject);
                    }
                }
                else if (LayerMask.LayerToName(hitObject.layer) == "Ground")
                {
                    attackRange.SendMessage("CheckEnemiesInRange", hitPosition);
                }
            }
        }
        else if (aKey.WasReleasedThisFrame())
        {
            attackRange.SendMessage("HideAttackRange");
        }
    }

    //Raycast
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
}
