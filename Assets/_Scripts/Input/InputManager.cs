using UnityEngine;
using UnityEngine.InputSystem;

// Routes mouse and keyboard input into player actions and targeting.
public class InputManager : MonoBehaviour
{
    // Handles mouse clicks and attack modifier key.
    private InputAction rightClick;
    private InputAction leftClick;
    private InputAction aKey;

    // Targets that receive movement or attack commands.
    private GameObject player;
    private GameObject attackRange;

    // Cached raycast info from the latest cursor check.
    private Vector3 hitPosition;
    private GameObject hitObject;

    private bool readyToAttack = false;

    private void Awake()
    {
        // Look up the controlled player and grab its helper objects.
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

        if (!readyToAttack)
        {
            attackRange.SendMessage("HideAttackRange");
        }

        // Right mouse button steers the player or triggers quick attacks.
        if (rightClick.WasPressedThisFrame())
        {
            if (readyToAttack)
            {
                readyToAttack = false;
            }
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

        // Holding A keeps the range indicator visible for targeted attacks.
        if (aKey.IsPressed())
        {
            readyToAttack = true;
            attackRange.SendMessage("ShowAttackRange");
        }

        // Left click confirms the target while in attack-readiness mode.
        if (leftClick.WasPressedThisFrame() && readyToAttack)
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
            readyToAttack = false;
        }
    }

    // Casts a ray from the cursor to see what the player is pointing at.
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
