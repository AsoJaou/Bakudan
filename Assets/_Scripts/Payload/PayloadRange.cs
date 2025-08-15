using UnityEngine;
using UnityEngine.Splines;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(SphereCollider))]
public class PayloadRange : MonoBehaviour
{
    private int segments = 100;
    private float radius = 10f;
    private bool payloadIsMoving;

    private LineRenderer lineRenderer;
    private SphereCollider payloadRangeCollider;
    private SplineAnimate splineAnimate;
    private GameObject payloadPercentageUI;
    [SerializeField] private GameObject sceneManager;

    private void Awake()
    {
        splineAnimate = GetComponentInParent<SplineAnimate>();
        lineRenderer = GetComponent<LineRenderer>();
        payloadRangeCollider = GetComponent<SphereCollider>();
        lineRenderer.useWorldSpace = false;
        lineRenderer.loop = true;
        lineRenderer.material = new Material(Shader.Find("Unlit/Color"));
        lineRenderer.material.color = new Color(0.553f, 0.784f, 0.851f, 0.7f);
        payloadPercentageUI = transform.parent.Find("Payload Percentage UI").gameObject;
    }

    private void Start()
    {
        payloadIsMoving = false;
        lineRenderer.positionCount = segments;

        for (int i = 0; i < segments; i++)
        {
            float angle = Mathf.Deg2Rad * i * (360f / segments);
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;
            lineRenderer.SetPosition(i, new Vector3(x, 0.2f, z));
        }

        payloadRangeCollider.radius = radius;
    }

    private void Update()
    {
        if (payloadIsMoving)
        {
            payloadPercentageUI.SendMessage("UpdatePayloadBar", splineAnimate.ElapsedTime / splineAnimate.Duration);
        }

        if (splineAnimate.ElapsedTime >= splineAnimate.Duration)
        {
            sceneManager.SendMessage("LoadScene", "VictoryScene");
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.transform.parent.CompareTag("Character"))
        {
            if (splineAnimate != null)
            {
                splineAnimate.enabled = true;
            }
            payloadIsMoving = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.transform.parent.CompareTag("Character"))
        {
            if (splineAnimate != null)
            {
                splineAnimate.enabled = false;
            }
            payloadIsMoving = false;
        }
    }
}
