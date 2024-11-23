using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int enemiesToSpawn;
    public int minObstaclePriority; // Minimum avoidance priority
    public int maxObstaclePriority; // Maximum avoidance priority

    private Map map;
    private ObstacleManager obstacleManager;
    private EnemyManager enemyManager;
    private Transform playerTransform;

    public void Initialize(Map mapReference, ObstacleManager obstacleManagerReference, Transform player)
    {
        Debug.Log("Initializing EnemySpawner...");
        map = mapReference;
        obstacleManager = obstacleManagerReference;
        playerTransform = player;

        enemyManager = FindObjectOfType<EnemyManager>();
        if (map == null || obstacleManager == null || enemyManager == null)
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

        Transform enemyParent = enemyManager.transform;
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
                    InitializeEnemy(enemy); // Moved initialization logic here
                    enemyManager.AddEnemy(enemy);
                }
            }
        }
    }

    // New helper method to initialize the enemy completely
    private void InitializeEnemy(GameObject enemy)
    {
        var enemyController = enemy.GetComponent<EnemyController>();
        if (enemyController != null)
        {
            // Ensure agent and settings are properly initialized
            enemyController.InitializeAgent();

            // Randomize obstacle avoidance priority
            int randomizedPriority = Random.Range(minObstaclePriority, maxObstaclePriority);
            enemyController.SetObstaclePriority(randomizedPriority);
            Debug.Log($"Enemy {enemy.name} obstacle priority set to {randomizedPriority}");

            // Assign player transform
            if (playerTransform != null)
            {
                enemyController.SetPlayerTransform(playerTransform);
            }
            else
            {
                Debug.LogError("Player Transform is null. Enemies cannot track the player.");
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
