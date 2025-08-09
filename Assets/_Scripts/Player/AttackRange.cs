using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(SphereCollider))]
public class AttackRange : MonoBehaviour
{
    private int segments = 100;

    private LineRenderer lineRenderer;
    private SphereCollider attackRangeCollider;
    private GameObject player;

    [SerializeField] private LayerMask enemyLayerMask;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        attackRangeCollider = GetComponent<SphereCollider>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = false;
        lineRenderer.loop = true;
        lineRenderer.widthMultiplier = 0.3f;
        lineRenderer.enabled = false;
        lineRenderer.material = new Material(Shader.Find("Unlit/Color"));
        lineRenderer.material.color = new Color(0.88f, 0.55f, 0.71f, 0.5f);
    }

    private void Update()
    {
        SetRadius(PlayerStats.Instance.AttackRange * 0.1f);
    }

    public void SetRadius(float attackRange)
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

    public void ShowAttackRange()
    {
        lineRenderer.enabled = true;
    }

    public void HideAttackRange()
    {
        lineRenderer.enabled = false;
    }

    private void OnTriggerEnter(Collider Enemy)
    {
        if (Enemy.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            GameManager.Instance.AddEnemyToRange(Enemy.gameObject.transform.parent.gameObject);
        }
    }

    private void OnTriggerExit(Collider Enemy)
    {
        if (Enemy.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            GameManager.Instance.RemoveEnemyFromRange(Enemy.gameObject.transform.parent.gameObject);
        }
    }

    void AttackClosestEnemy(Vector3 HitPosition)
    {
        GameObject closestEnemy = null;
        float closestEnemyDistance = Mathf.Infinity;
        foreach (GameObject enemy in GameManager.Instance.EnemiesInRange)
        {
            float distance = Vector3.Distance(HitPosition, enemy.transform.position);
            if (distance < closestEnemyDistance)
            {
                closestEnemyDistance = distance;
                closestEnemy = enemy;
            }
        }

        NormalAttack(closestEnemy);
    }

    void NormalAttack(GameObject target)
    {
        GameObject character = GameObject.FindGameObjectWithTag("Character");
        player.SendMessage("StopMoving");
        transform.parent.LookAt(target.transform.position);
        GameObject NormalAttackInstance = Instantiate(character.transform.Find("Normal Attack").gameObject);
        NormalAttackInstance.transform.position = transform.position;
        NormalAttackInstance.transform.LookAt(target.transform);
        NormalAttackInstance.SendMessage("AttackTarget", target, SendMessageOptions.DontRequireReceiver);
    }


}
