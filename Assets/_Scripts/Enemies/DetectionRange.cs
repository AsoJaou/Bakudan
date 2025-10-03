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
            Debug.Log("Detected " + other.gameObject.name);
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
            obj.SendMessage("HealthChange", -50, SendMessageOptions.DontRequireReceiver);
        }

        Destroy(transform.parent.gameObject);
    }
}