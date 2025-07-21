using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(SphereCollider))]
public class AttackRange : MonoBehaviour
{
    private int segments = 100;

    private LineRenderer lineRenderer;
    private SphereCollider attackRangeCollider;

    [SerializeField] private LayerMask enemyLayerMask;
    private List<GameObject> enemiesInRange = new List<GameObject>();

    private void Awake()
    {
        attackRangeCollider = GetComponent<SphereCollider>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = false;
        lineRenderer.loop = true;
        lineRenderer.widthMultiplier = 0.3f;
        lineRenderer.enabled = false;
    }

    private void Update()
    {
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
        lineRenderer.enabled = true;
    }

    public void HideAttackRange()
    {
        lineRenderer.enabled = false;
    }

    private void OnTriggerEnter(Collider Enemy)
    {
        if (Enemy.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (!enemiesInRange.Contains(Enemy.gameObject))
            {

                enemiesInRange.Add(Enemy.gameObject.transform.parent.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider Enemy)
    {
        if (enemiesInRange.Contains(Enemy.gameObject.transform.parent.gameObject))
        {
            enemiesInRange.Remove(Enemy.gameObject.transform.parent.gameObject);
        }
    }
}
