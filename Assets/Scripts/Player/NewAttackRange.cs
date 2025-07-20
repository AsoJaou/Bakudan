using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class NewAttackRange : MonoBehaviour
{
    private float radius = 5f;
    private int segments = 100;

    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = false;
        lineRenderer.loop = true;
        lineRenderer.widthMultiplier = 0.3f;
        lineRenderer.enabled = false;
    }

    void Start()
    {
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

    public void ShowAttackRange()
    {
        lineRenderer.enabled = true;
    }

    public void HideAttackRange()
    {
        lineRenderer.enabled = false;
    }
}
