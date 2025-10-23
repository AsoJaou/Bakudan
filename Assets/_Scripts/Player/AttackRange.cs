using System.Collections.Generic;
using UnityEngine;

// Maintains the player's attack detection area and launches basic attacks.
[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(SphereCollider))]
public class AttackRange : MonoBehaviour
{
    private int segments = 100;

    private LineRenderer lineRenderer;
    private SphereCollider attackRangeCollider;
    private GameObject player;
    private PlayerController playerController;
    private GameObject normalAttackPrefab;

    [SerializeField] private LayerMask enemyLayerMask;

    private void Awake()
    {
        // Locate required scene references and cache rendering components.
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>();
        }

        var character = GameObject.FindGameObjectWithTag("Character");
        if (character != null)
        {
            var normalAttackTransform = character.transform.Find("Normal Attack");
            if (normalAttackTransform != null)
            {
                normalAttackPrefab = normalAttackTransform.gameObject;
            }
        }

        attackRangeCollider = GetComponent<SphereCollider>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = false;
        lineRenderer.loop = true;
        lineRenderer.widthMultiplier = 0.3f;
        lineRenderer.enabled = false;
        lineRenderer.material.color = new Color(0.88f, 0.55f, 0.71f, 0.5f);
    }

    private void Update()
    {
        // Keep the visual and collider radius aligned with current stats.
        SetRadius(PlayerStats.Instance.AttackRange * 0.1f);
    }

    public void SetRadius(float attackRange)
    {
        lineRenderer.positionCount = segments;

        float angleStep = 360f / segments;

        for (int i = 0; i < segments; i++)
        {
            float angle = Mathf.Deg2Rad * i * angleStep;
            float x = Mathf.Cos(angle) * attackRange;
            float z = Mathf.Sin(angle) * attackRange;
            lineRenderer.SetPosition(i, new Vector3(x, 0, z));
        }

        attackRangeCollider.radius = attackRange;
    }

    public void ShowAttackRange()
    {
        // Reveal the range indicator while the player is aiming.
        lineRenderer.enabled = true;
    }

    public void HideAttackRange()
    {
        // Hide the visual when the player is not preparing an attack.
        lineRenderer.enabled = false;
    }

    private void OnTriggerEnter(Collider Enemy)
    {
        // Track enemies that slip into the attack sphere.
        if (Enemy.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            GameManager.Instance.AddEnemyToRange(Enemy.gameObject.transform.parent.gameObject);
        }
    }

    private void OnTriggerExit(Collider Enemy)
    {
        // Stop considering enemies once they leave our radius.
        if (Enemy.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            GameManager.Instance.RemoveEnemyFromRange(Enemy.gameObject.transform.parent.gameObject);
        }
    }

    private void AttackClosestEnemy(Vector3 HitPosition)
    {
        // Pick whichever enemy sits nearest to the cursor impact point.
        GameObject closestEnemy = null;
        float closestEnemyDistance = Mathf.Infinity;
        foreach (GameObject enemy in GameManager.Instance.EnemiesInRange)
        {
            float distance = Vector3.Distance(HitPosition, enemy.transform.position);
            if (distance < closestEnemyDistance)
            {
                closestEnemyDistance = distance;
                closestEnemy = enemy;
            }
        }

        NormalAttack(closestEnemy);
    }

    public void NormalAttack(GameObject target)
    {
        if (target == null)
        {
            return;
        }

        // Cut movement so attacks fire from a stationary position.
        if (playerController != null)
        {
            playerController.StopMoving();
        }

        transform.parent.LookAt(target.transform.position);

        if (normalAttackPrefab == null)
        {
            Debug.LogWarning("NormalAttack prefab not assigned.");
            return;
        }

        // Spawn the projectile and send it toward the chosen target.
        GameObject normalAttackInstance = Instantiate(normalAttackPrefab);
        normalAttackInstance.transform.position = transform.position;
        normalAttackInstance.transform.LookAt(target.transform);

        NormalAttack attackBehaviour = normalAttackInstance.GetComponent<NormalAttack>();
        if (attackBehaviour != null)
        {
            attackBehaviour.AttackTarget(target);
        }
    }
}
