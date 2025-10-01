using System.Collections;
using UnityEngine;

public class NormalAttack : MonoBehaviour
{
    //Normal Attack Variables
    private float baseAttackSpeed = 15f;
    private Vector3 currentPos;
    private Vector3 displacement;
    private Vector3 direction;

    private Vector3 targetPosition;

    public void AttackTarget(GameObject target)
    {
        if (target != null)
        {
            targetPosition = target.transform.position;
        }
        StartCoroutine(MoveToPosition(targetPosition, target));
    }

    IEnumerator MoveToPosition(Vector3 targetPosition, GameObject target)
    {
        if (target == null)
        {
            yield break;
        }
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.LookAt(new Vector3(targetPosition.x, transform.position.y, targetPosition.z));
            currentPos = transform.position;
            displacement = targetPosition - currentPos;
            direction = displacement.normalized * Time.deltaTime;
            transform.position += direction * baseAttackSpeed;

            yield return null;
        }

        if (target != null)
        {
            target.SendMessage("HealthChange", -PlayerStats.Instance.AttackDamage);
        }

        Destroy(gameObject);
    }
}
