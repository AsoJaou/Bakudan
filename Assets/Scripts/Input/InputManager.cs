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

    private float leftClickDetection = 0f;
    private float leftClickOffset = 0.1f;

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
        Debug.Log(leftClickDetection);
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
                if (GameManager.Instance.EnemiesInRange.Contains(hitObject.transform.parent.gameObject))
                {
                    attackRange.SendMessage("NormalAttack", hitObject.transform.parent.gameObject);
                }
                else
                {
                    player.SendMessage("MoveToAttack", hitObject.transform.parent.gameObject);
                }
            }
        }

        //A Key Input
        if (aKey.IsPressed())
        {
            leftClickDetection = leftClickOffset;
            attackRange.SendMessage("ShowAttackRange");
        }
        else if (aKey.WasReleasedThisFrame())
        {
            attackRange.SendMessage("HideAttackRange");
        }

        if (leftClickDetection > 0f)
        {
            leftClickDetection -= Time.deltaTime;
        }

        if (leftClick.WasPressedThisFrame() && leftClickDetection > 0f)
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
                if (GameManager.Instance.EnemiesInRange.Count > 0f)
                {
                    attackRange.SendMessage("AttackClosestEnemy", hitPosition);
                }
                else
                {
                    player.SendMessage("MoveToPosition", hitPosition);
                }
            }
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