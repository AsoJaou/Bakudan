using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(SphereCollider))]
public class AttackRange : MonoBehaviour
{
    private float attackRange;
    private int segments = 100;

    private LineRenderer lineRenderer;
    private SphereCollider attackRangeCollider;

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
        attackRange = PlayerStats.Instance.AttackRange;
        attackRangeCollider.radius = attackRange;
        DrawCircle();
    }

    public void DrawCircle()
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
    }

    public void SetRadius(float newRadius)
    {
        attackRange = newRadius;
        attackRangeCollider.radius = newRadius;
        DrawCircle();
    }

    public void ShowAttackRange()
    {
        lineRenderer.enabled = true;
    }

    public void HideAttackRange()
    {
        lineRenderer.enabled = false;
    }
}
