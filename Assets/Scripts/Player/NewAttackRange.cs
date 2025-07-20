using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CircleRenderer : MonoBehaviour
{
    private float radius = 5f;
    private int segments = 100;
    private GameObject AttackRange;

    private LineRenderer lineRenderer;

    void Start()
    {
        AttackRange = GameObject.Find("Attack Range");
        AttackRange.SetActive(false);
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = false;
        lineRenderer.loop = true;
        lineRenderer.widthMultiplier = 0.3f;

        DrawCircle();
    }

    public void DrawCircle()
    {
        lineRenderer.positionCount = segments;

        float angleStep = 360f / segments;

        for (int i = 0; i < segments; i++)
        {
            float angle = Mathf.Deg2Rad * i * angleStep;
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;
            lineRenderer.SetPosition(i, new Vector3(x, 0, z));
        }
    }

    // Optional: Call this to update radius dynamically
    public void SetRadius(float newRadius)
    {
        radius = newRadius;
        DrawCircle();
    }
}
