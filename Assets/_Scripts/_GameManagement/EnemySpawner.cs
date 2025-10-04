using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Vector2 spawnIntervalRange = new Vector2(1.5f, 3.5f);
    [SerializeField] private int spawnCountPerWave = 1;
    [SerializeField] private float spawnHeightOffset = 0f;
    [SerializeField] private bool followTarget = true;
    [SerializeField] private Transform followTransform;
    [SerializeField] private Vector3 followOffset = Vector3.zero;

    private BoxCollider spawnArea;
    private float spawnTimer;
    private float initialYPosition;

    private void Awake()
    {
        spawnArea = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        if (spawnArea != null)
        {
            spawnArea.isTrigger = true;
        }

        if (followTarget && followTransform == null)
        {
            var cam = Camera.main;
            if (cam != null)
            {
                followTransform = cam.transform;
            }
        }

        initialYPosition = transform.position.y;
        spawnTimer = GetRandomInterval();
    }

    private void Update()
    {
        if (followTarget && followTransform != null)
        {
            var position = transform.position;
            position.x = followTransform.position.x + followOffset.x;
            position.z = followTransform.position.z + followOffset.z;
            position.y = initialYPosition + followOffset.y;
            transform.position = position;
        }

        if (enemyPrefab == null)
        {
            return;
        }

        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            SpawnWave();
            spawnTimer = GetRandomInterval();
        }
    }

    private void SpawnWave()
    {
        if (enemyPrefab == null || spawnArea == null)
        {
            return;
        }

        for (int i = 0; i < Mathf.Max(1, spawnCountPerWave); i++)
        {
            Vector3 position = GetRandomEdgePosition(spawnArea.bounds);
            position.y = spawnArea.bounds.center.y + spawnHeightOffset;
            Instantiate(enemyPrefab, position, Quaternion.identity);
        }
    }

    private float GetRandomInterval()
    {
        if (spawnIntervalRange.y <= 0f)
        {
            return 1f;
        }

        float min = Mathf.Max(0.1f, spawnIntervalRange.x);
        float max = Mathf.Max(min, spawnIntervalRange.y);
        return Random.Range(min, max);
    }

    private static Vector3 GetRandomEdgePosition(Bounds bounds)
    {
        int edge = Random.Range(0, 4);
        float x;
        float z;

        switch (edge)
        {
            case 0: // top edge (positive Z)
                x = Random.Range(bounds.min.x, bounds.max.x);
                z = bounds.max.z;
                break;
            case 1: // bottom edge (negative Z)
                x = Random.Range(bounds.min.x, bounds.max.x);
                z = bounds.min.z;
                break;
            case 2: // left edge (negative X)
                x = bounds.min.x;
                z = Random.Range(bounds.min.z, bounds.max.z);
                break;
            default: // right edge (positive X)
                x = bounds.max.x;
                z = Random.Range(bounds.min.z, bounds.max.z);
                break;
        }

        return new Vector3(x, bounds.center.y, z);
    }

    private void OnDrawGizmosSelected()
    {
        var box = GetComponent<BoxCollider>();
        if (box == null)
        {
            return;
        }

        Gizmos.color = Color.yellow;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(box.center, box.size);
    }
}