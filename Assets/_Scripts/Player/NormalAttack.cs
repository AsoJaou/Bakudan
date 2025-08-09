using System.Collections;
using UnityEngine;

public class NormalAttack : MonoBehaviour
{
    //Normal Attack Variables
    private float baseAttackSpeed = 15f;
    private Vector3 currentPos;
    private Vector3 displacement;
    private Vector3 direction;

    void AttackTarget(GameObject target)
    {
        StartCoroutine(MoveToPosition(target));
    }

    IEnumerator MoveToPosition(GameObject target)
    {
        while (Vector3.Distance(transform.position, target.transform.position) > 0.1f)
        {
            transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
            currentPos = transform.position;
            displacement = target.transform.position - currentPos;
            direction = displacement.normalized * Time.deltaTime;
            transform.position += direction * baseAttackSpeed;

            yield return null;
        }

        target.SendMessage("HealthChange", -PlayerStats.Instance.AttackDamage);

        Destroy(gameObject);
    }
}
