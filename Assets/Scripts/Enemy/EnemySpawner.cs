using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemyPrefabs;
    [SerializeField] private EnemySpawnerSettings settings;

    private Map map;
    private EnemyManager enemyManager;
    private Transform playerTransform;

    public void Initialize(Map mapReference, Transform player)
    {
        Debug.Log("Initializing EnemySpawner...");

        map = mapReference;
        playerTransform = player;

        enemyManager = EnemyManager.Instance;

        if (map == null || enemyManager == null)
        {
            Debug.LogError("EnemySpawner dependencies are missing.");
            return;
        }

        if (enemyPrefabs == null || enemyPrefabs.Count == 0)
        {
            Debug.LogError("No enemy prefabs assigned to EnemySpawner.");
            return;
        }

        SpawnEnemies();
        Debug.Log("EnemySpawner initialized.");
    }

    private void SpawnEnemies()
    {
        List<Tile> tiles = new List<Tile>(map.GetAllTiles());
        if (tiles.Count == 0)
        {
            Debug.LogWarning("No tiles available for spawning enemies.");
            return;
        }

        int enemiesPerTile = Mathf.CeilToInt((float)settings.EnemiesToSpawn / tiles.Count);

        foreach (var tile in tiles)
        {
            SpawnEnemiesOnTile(tile, enemiesPerTile);
        }
    }

    private void SpawnEnemiesOnTile(Tile tile, int enemiesPerTile)
    {
        for (int i = 0; i < enemiesPerTile; i++)
        {
            Vector3? validPosition = FindValidPosition(tile);

            if (validPosition != null)
            {
                var prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
                var enemy = Instantiate(prefab, validPosition.Value, Quaternion.identity, enemyManager.transform);

                InitializeEnemy(enemy);
                enemyManager.AddEnemy(enemy);
            }
            else
            {
                Debug.LogWarning($"Failed to find valid spawn position on tile {tile.name}.");
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
        if (enemyController != null)
        {
            enemyController.SetPlayerTransform(playerTransform);
            enemyController.InitializeAgent();
        }
        else
        {
            Debug.LogWarning($"{enemy.name} does not have an EnemyController component.");
        }
    }

    private Vector3? FindValidPosition(Tile tile)
    {
        for (int attempt = 0; attempt < settings.MaxRetries; attempt++)
        {
            Vector3 position = tile.GetRandomPosition(settings.EnemyRadius);

            if (IsPositionValid(position, settings.EnemyRadius))
            {
                return position;
            }
        }
        return null;
    }

    private bool IsPositionValid(Vector3 position, float radius)
    {
        int obstacleLayer = 1 << LayerMask.NameToLayer("Obstacle");
        return Physics.OverlapSphere(position, radius, obstacleLayer).Length == 0;
    }
}
