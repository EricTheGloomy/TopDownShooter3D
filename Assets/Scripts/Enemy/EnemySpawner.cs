// Scripts/Enemy/EnemySpawner.cs
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemyPrefabs;       // Existing enemy prefabs
    [SerializeField] private EnemySpawnConfig spawnConfig;       // New spawn configuration
    [SerializeField] private EnemySpawnerSettings settings;      // Existing spawner settings

    private Map map;
    private Transform playerTransform;
    private EnemyManager enemyManager;

    public void Initialize(Map mapReference, Transform player)
    {
        if (mapReference == null || spawnConfig == null || settings == null)
        {
            Debug.LogError("EnemySpawner dependencies are not properly assigned.");
            return;
        }

        map = mapReference;
        playerTransform = player;
        enemyManager = EnemyManager.Instance;

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

        // Exclude tile occupied by the player
        Tile playerTile = map.GetTileAtPosition(playerTransform.position);
        tiles.Remove(playerTile);

        if (tiles.Count == 0)
        {
            Debug.LogWarning("All tiles are occupied by the player. No enemies can be spawned.");
            return;
        }

        // Shuffle tiles for random assignment
        ShuffleTiles(tiles);

        // Calculate initial tile allocations based on percentages
        Dictionary<EnemySpawnConfig.SpawnPattern, int> patternTileCounts = new();
        int totalAssignedTiles = 0;

        foreach (var pattern in spawnConfig.Patterns)
        {
            int count = Mathf.FloorToInt(pattern.Percentage / 100f * tiles.Count);
            patternTileCounts[pattern] = count;
            totalAssignedTiles += count;
        }

        // Distribute leftover tiles to patterns with higher percentages
        int leftoverTiles = tiles.Count - totalAssignedTiles;
        var orderedPatterns = spawnConfig.Patterns.OrderByDescending(p => p.Percentage).ToList();
        for (int i = 0; i < leftoverTiles; i++)
        {
            patternTileCounts[orderedPatterns[i % orderedPatterns.Count]]++;
        }

        // Spawn enemies for each pattern
        foreach (var kvp in patternTileCounts)
        {
            Debug.Log($"Pattern '{kvp.Key.PatternName}' assigned to {kvp.Value} tiles.");
            var pattern = kvp.Key;
            int count = kvp.Value;

            var patternTiles = tiles.GetRange(0, Mathf.Min(count, tiles.Count));
            tiles.RemoveRange(0, Mathf.Min(count, tiles.Count));

            foreach (var tile in patternTiles)
            {
                SpawnPatternOnTile(tile, pattern);
            }
        }

        if (tiles.Count > 0)
        {
            Debug.LogWarning("Some tiles are left unassigned due to misconfigured percentages.");
        }
    }

    private void SpawnPatternOnTile(Tile tile, EnemySpawnConfig.SpawnPattern pattern)
    {
        for (int i = 0; i < pattern.EnemiesToSpawn.Count; i++)
        {
            GameObject enemyPrefab = pattern.EnemiesToSpawn[i];
            int countToSpawn = pattern.EnemyCounts[i];

            for (int count = 0; count < countToSpawn; count++)
            {
                Vector3? validPosition = FindValidPositionOnTile(tile);

                if (validPosition.HasValue)
                {
                    var enemy = Instantiate(enemyPrefab, validPosition.Value, Quaternion.identity, enemyManager.transform);

                    Debug.Log($"Spawned {enemyPrefab.name} at {validPosition.Value} on tile {tile.name}.");
                    InitializeEnemy(enemy);
                    enemyManager.AddEnemy(enemy);
                }
                else
                {
                    Debug.LogWarning($"Failed to find valid spawn position for {enemyPrefab.name} on tile {tile.name}.");
                }
            }
        }
    }

    private void InitializeEnemy(GameObject enemy)
    {
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

    private Vector3? FindValidPositionOnTile(Tile tile)
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
        int obstacleLayer = 1 << LayerMask.NameToLayer(settings.obstacleLayerName);
        return Physics.OverlapSphere(position, radius, obstacleLayer).Length == 0;
    }

    private void ShuffleTiles(List<Tile> tiles)
    {
        for (int i = tiles.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            Tile temp = tiles[i];
            tiles[i] = tiles[randomIndex];
            tiles[randomIndex] = temp;
        }
    }
}
