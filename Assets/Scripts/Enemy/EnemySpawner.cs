using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int enemiesToSpawn;
    public int minObstaclePriority; // Minimum avoidance priority
    public int maxObstaclePriority; // Maximum avoidance priority

    private Map map;
    private ObstacleManager obstacleManager;

    public void Initialize(Map mapReference, ObstacleManager obstacleManagerReference)
    {
        Debug.Log("Initializing EnemySpawner...");
        map = mapReference;
        obstacleManager = obstacleManagerReference;

        if (map == null || obstacleManager == null)
        {
            Debug.LogError("EnemySpawner dependencies are missing.");
            return;
        }

        SpawnEnemies();

        Debug.Log("EnemySpawner initialized.");
    }

    private void SpawnEnemies()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("EnemyPrefab is not assigned.");
            return;
        }

        Transform enemyParent = FindObjectOfType<EnemyManager>()?.transform;
        if (enemyParent == null)
        {
            Debug.LogError("EnemyManager GameObject is missing.");
            return;
        }

        List<Tile> tiles = new List<Tile>(map.GetAllTiles());
        int enemiesPerTile = Mathf.CeilToInt((float)enemiesToSpawn / tiles.Count);

        foreach (var tile in tiles)
        {
            for (int i = 0; i < enemiesPerTile; i++)
            {
                Vector3? validPosition = FindValidPosition(tile);

                if (validPosition.HasValue)
                {
                    GameObject enemy = Instantiate(enemyPrefab, validPosition.Value, Quaternion.identity, enemyParent);

                    // Randomize obstacle avoidance priority
                    EnemyController enemyController = enemy.GetComponent<EnemyController>();
                    if (enemyController != null)
                    {
                        enemyController.SetObstaclePriority(Random.Range(minObstaclePriority, maxObstaclePriority));
                    }

                    FindObjectOfType<EnemyManager>()?.AddEnemy(enemy);
                }
            }
        }
    }

    private Vector3? FindValidPosition(Tile tile)
    {
        int maxRetries = 50;
        float enemyRadius = 0.25f; // Adjust based on enemy size
        int obstacleLayerMask = 1 << LayerMask.NameToLayer("Obstacle");

        for (int attempt = 0; attempt < maxRetries; attempt++)
        {
            Vector3 tileCenter = tile.transform.position;
            Vector3 tileSize = tile.TileSize;

            float randomX = Random.Range(tileCenter.x - tileSize.x / 2, tileCenter.x + tileSize.x / 2);
            float randomZ = Random.Range(tileCenter.z - tileSize.z / 2, tileCenter.z + tileSize.z / 2);
            Vector3 spawnPosition = new Vector3(randomX, tileCenter.y + 1, randomZ);

            if (Physics.OverlapSphere(spawnPosition, enemyRadius, obstacleLayerMask).Length == 0)
            {
                return spawnPosition;
            }
        }

        Debug.LogWarning($"No valid position found for tile {tile.name} after {maxRetries} attempts.");
        return null;
    }
}
