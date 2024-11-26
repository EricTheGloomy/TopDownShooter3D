// Scripts/Obstacle/ObstacleSpawner.cs
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private ObstacleConfig obstacleConfig;       // Existing config
    [SerializeField] private ObstacleSpawnConfig spawnConfig;     // New spawn configuration

    private Map map;
    private readonly Dictionary<Tile, List<GameObject>> tileObstacles = new();

    public void Initialize(Map mapReference)
    {
        if (mapReference == null || obstacleConfig == null || spawnConfig == null)
        {
            Debug.LogError("ObstacleSpawner dependencies are not properly assigned.");
            return;
        }

        map = mapReference;
        SpawnObstacles();
        Debug.Log("ObstacleSpawner initialized.");
    }

    private void SpawnObstacles()
    {
        List<Tile> tiles = new List<Tile>(map.GetAllTiles());
        if (tiles.Count == 0)
        {
            Debug.LogWarning("No tiles available for spawning obstacles.");
            return;
        }

        // Shuffle tiles for random assignment
        ShuffleTiles(tiles);

        // Calculate initial tile allocations based on percentages
        Dictionary<ObstacleSpawnConfig.SpawnPattern, int> patternTileCounts = new();
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

        // Spawn obstacles for each pattern
        foreach (var kvp in patternTileCounts)
        {
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

    private void SpawnPatternOnTile(Tile tile, ObstacleSpawnConfig.SpawnPattern pattern)
    {
        if (!tileObstacles.ContainsKey(tile))
        {
            tileObstacles[tile] = new List<GameObject>();
        }

        for (int i = 0; i < pattern.ObstaclesToSpawn.Count; i++)
        {
            for (int count = 0; count < pattern.ObstacleCounts[i]; count++)
            {
                Vector3? validPosition = FindValidPositionOnTile(tile);

                if (validPosition.HasValue)
                {
                    GameObject obstacle = InstantiateAndRegisterObstacle(
                        validPosition.Value,
                        tileObstacles[tile].Count + 1,
                        tile,
                        pattern.ObstaclesToSpawn[i]
                    );
                    tileObstacles[tile].Add(obstacle);
                }
                else
                {
                    Debug.LogWarning($"Could not find a valid position for an obstacle on tile {tile.name}.");
                }
            }
        }
    }

    private GameObject InstantiateAndRegisterObstacle(Vector3 position, int index, Tile tile, GameObject prefab)
    {
        GameObject obstacle = Instantiate(prefab, position, Quaternion.identity);

        AdjustTransformForTile(obstacle.transform, tile.transform);

        obstacle.name = $"Obstacle_{index}";

        Obstacle obstacleScript = obstacle.GetComponent<Obstacle>();
        if (obstacleScript != null)
        {
            position.y += obstacleScript.ObstacleHeight / 2; // Adjust position
            obstacle.transform.position = position;
            obstacle.layer = LayerMask.NameToLayer(obstacleConfig.ObstacleLayer);
        }
        else
        {
            Debug.LogError($"Obstacle {obstacle.name} is missing the Obstacle script.");
        }

        ObstacleManager.Instance?.AddObstacle(obstacle);

        return obstacle;
    }

    private void AdjustTransformForTile(Transform obstacleTransform, Transform tileTransform)
    {
        obstacleTransform.SetParent(tileTransform, true);
        //Not needed anymore ??? 
        //Vector3 localScale = obstacleConfig.DefaultObstaclePrefab.transform.localScale;
        //localScale.x /= tileTransform.localScale.x;
        //localScale.y /= tileTransform.localScale.y;
        //localScale.z /= tileTransform.localScale.z;
        //obstacleTransform.localScale = localScale;
    }

    private Vector3? FindValidPositionOnTile(Tile tile)
    {
        for (int attempt = 0; attempt < obstacleConfig.MaxRetries; attempt++)
        {
            Vector3 position = tile.GetRandomPosition(obstacleConfig.DefaultObstacleRadius);
            if (IsPositionValid(position, obstacleConfig.DefaultObstacleRadius))
            {
                return position;
            }
        }
        return null;
    }

    private bool IsPositionValid(Vector3 position, float radius)
    {
        int obstacleLayer = 1 << LayerMask.NameToLayer(obstacleConfig.ObstacleLayer);
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
