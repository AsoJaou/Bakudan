using UnityEngine;

public class NormalAttackController : MonoBehaviour
{
    private GameObject attackRange;

    private void Awake()
    {
        attackRange = transform.Find("Attack Range").gameObject;
    }
}
