using System.Collections;
using UnityEngine;

// Moves a projectile toward its target and applies damage on contact.
public class NormalAttack : MonoBehaviour
{
    private const float baseAttackSpeed = 15f;

    private GameObject currentTarget;
    private Transform targetTransform;

    public void AttackTarget(GameObject target)
    {
        currentTarget = target;
        targetTransform = currentTarget != null ? currentTarget.transform : null;

        if (targetTransform == null)
        {
            Destroy(gameObject);
            return;
        }

        // Start chasing the target until it is hit or lost.
        StartCoroutine(FollowTarget());
    }

    private IEnumerator FollowTarget()
    {
        while (true)
        {
            if (currentTarget == null || targetTransform == null)
            {
                break;
            }

            Vector3 targetPosition = targetTransform.position;
            Vector3 planarTarget = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, baseAttackSpeed * Time.deltaTime);
            transform.LookAt(planarTarget);

            if (Vector3.Distance(transform.position, targetPosition) <= 0.1f)
            {
                if (currentTarget != null)
                {
                    // Notify the target that it has taken standard attack damage.
                    currentTarget.SendMessage("HealthChange", -PlayerStats.Instance.AttackDamage, SendMessageOptions.DontRequireReceiver);
                }
                break;
            }

            yield return null;
        }

        Destroy(gameObject);
    }
}
