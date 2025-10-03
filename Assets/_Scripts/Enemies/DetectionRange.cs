using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionRange : MonoBehaviour
{
    private List<GameObject> detectedObjects = new List<GameObject>();

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
        yield return new WaitForSeconds(2f);
        Detonate();
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
                Debug.Log("Payload hit!");
            }
        }

        GameManager.Instance.RemoveEnemyFromRange(transform.parent.gameObject);
        Destroy(transform.parent.gameObject);
    }
}