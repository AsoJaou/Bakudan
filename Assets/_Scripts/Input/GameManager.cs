using System.Collections.Generic;
using UnityEngine;

// Tracks enemies that are currently inside the player attack range.
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Status")]
    [SerializeField] private List<GameObject> enemiesInRange = new List<GameObject>();

    public List<GameObject> EnemiesInRange => enemiesInRange;

    private void Awake()
    {
        // Ensure there is a single shared manager for range bookkeeping.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void AddEnemyToRange(GameObject enemy)
    {
        if (!enemiesInRange.Contains(enemy))
        {
            enemiesInRange.Add(enemy);
        }
    }
    public void RemoveEnemyFromRange(GameObject enemy)
    {
        if (enemiesInRange.Contains(enemy))
        {
            enemiesInRange.Remove(enemy);
        }
    }
}