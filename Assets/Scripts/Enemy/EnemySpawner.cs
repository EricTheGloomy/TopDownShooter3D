using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Prefab & Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private EnemySpawnerSettings settings; // ScriptableObject for configurations

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

        if (tiles.Count == 0)
        {
            Debug.LogWarning("No tiles found on the map for spawning enemies.");
            return;
        }

        int enemiesPerTile = Mathf.CeilToInt((float)settings.enemiesToSpawn / tiles.Count);

        foreach (var tile in tiles)
        {
            SpawnEnemiesOnTile(tile, enemiesPerTile, enemyParent);
        }
    }

    private void SpawnEnemiesOnTile(Tile tile, int enemiesPerTile, Transform enemyParent)
    {
        for (int i = 0; i < enemiesPerTile; i++)
        {
            Vector3? validPosition = FindValidPosition(tile);

            if (validPosition.HasValue)
            {
                GameObject enemy = Instantiate(enemyPrefab, validPosition.Value, Quaternion.identity, enemyParent);
                InitializeEnemy(enemy);
                enemyManager.AddEnemy(enemy);
            }
        }
    }

    private void InitializeEnemy(GameObject enemy)
    {
        if (enemy == null)
        {
            Debug.LogError("Attempted to initialize a null enemy.");
            return;
        }

        var enemyController = enemy.GetComponent<EnemyController>();
        if (enemyController == null)
        {
            Debug.LogWarning($"Enemy {enemy.name} does not have an EnemyController component.");
            return;
        }

        // Set obstacle avoidance priority
        int randomizedPriority = Random.Range(settings.minObstaclePriority, settings.maxObstaclePriority);
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

        // Initialize agent-specific properties
        enemyController.InitializeAgent();
    }

    private Vector3? FindValidPosition(Tile tile)
    {
        int maxRetries = settings.maxRetries;
        float enemyRadius = settings.enemyRadius;
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
