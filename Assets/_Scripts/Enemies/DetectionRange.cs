using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionRange : MonoBehaviour
{
    private GameObject TrainingDummy;
    private LineRenderer lineRenderer;
    private SphereCollider attackRangeCollider;
    private EnemyStats enemyStats;
    private int segments = 100;
    private List<GameObject> detectedObjects = new List<GameObject>();

    private void Awake()
    {
        TrainingDummy = transform.parent.gameObject;
        enemyStats = TrainingDummy.GetComponent<EnemyStats>();

        attackRangeCollider = GetComponent<SphereCollider>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = false;
        lineRenderer.loop = true;
        lineRenderer.widthMultiplier = 0.3f;
        lineRenderer.enabled = false;
        lineRenderer.material.color = Color.red;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player Collider") || other.CompareTag("Payload Collider"))
        {
            detectedObjects.Add(other.gameObject);
            StartCoroutine(ExplosionCountdown());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player Collider") || other.CompareTag("Payload Collider"))
        {
            detectedObjects.Remove(other.gameObject);
        }
    }

    IEnumerator ExplosionCountdown()
    {
        SetRadius(enemyStats.attackRange * 0.1f);
        lineRenderer.enabled = true;
        yield return new WaitForSeconds(2f);
        Detonate();
    }

    private void SetRadius(float attackRange)
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

    private void Detonate()
    {
        Debug.Log("Detonated!");
        foreach (var obj in detectedObjects)
        {
            if (obj.CompareTag("Player Collider"))
            {
                PlayerStats.Instance.HealthChange(-50f);
            }
            else if (obj.CompareTag("Payload Collider"))
            {
                obj.transform.parent.gameObject.GetComponent<PayloadStats>().HealthChange(-50f);
            }
        }

        GameManager.Instance.RemoveEnemyFromRange(transform.parent.gameObject);
        Destroy(transform.parent.gameObject);
    }
}