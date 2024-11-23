// Scripts/Obstacle/ObstacleSpawner.cs
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public int obstaclesPerTile = 2;

    private Map map;
    private Dictionary<Tile, List<GameObject>> tileObstacles = new Dictionary<Tile, List<GameObject>>();
    private ObstacleManager obstacleManager; // Reference to ObstacleManager

    public void Initialize(Map mapReference, ObstacleManager obstacleManagerReference)
    {
        Debug.Log("Initializing ObstacleSpawner...");
        map = mapReference;
        obstacleManager = obstacleManagerReference;

        if (map == null || obstacleManager == null)
        {
            Debug.LogError("ObstacleSpawner dependencies are not assigned.");
            return;
        }

        SpawnObstacles();

        Debug.Log("ObstacleSpawner initialized.");
    }

    private void SpawnObstacles()
    {
        if (obstaclePrefab == null)
        {
            Debug.LogError("ObstaclePrefab is not assigned.");
            return;
        }

        foreach (var tile in map.GetAllTiles())
        {
            if (!tileObstacles.ContainsKey(tile))
            {
                tileObstacles[tile] = new List<GameObject>();
            }

            for (int i = 0; i < obstaclesPerTile; i++)
            {
                Vector3? validPosition = FindValidPositionOnTile(tile);

                if (validPosition.HasValue)
                {
                    GameObject obstacle = InstantiateAndRegisterObstacle(validPosition.Value, tileObstacles[tile].Count + 1, tile);
                    tileObstacles[tile].Add(obstacle);
                }
                else
                {
                    Debug.LogWarning($"Could not find a valid position for an obstacle on tile {tile.name}.");
                }
            }
        }

        Debug.Log($"Obstacles spawned for {tileObstacles.Count} tiles.");
    }

    // Combines functionality of both methods for spawning and registering an obstacle
    private GameObject InstantiateAndRegisterObstacle(Vector3 position, int index, Tile tile)
    {
        var obstacle = Instantiate(obstaclePrefab, position, Quaternion.identity);
        obstacle.name = $"Obstacle_{index}";
        obstacle.transform.SetParent(tile.transform); // Make it a child of the tile

        Obstacle obstacleScript = obstacle.GetComponent<Obstacle>();
        if (obstacleScript != null)
        {
            position.y += obstacleScript.ObstacleHeight / 2;
            obstacle.transform.position = position;

            // Assign the obstacle to the "Obstacle" layer
            obstacle.layer = LayerMask.NameToLayer("Obstacle");
        }
        else
        {
            Debug.LogError($"Obstacle {obstacle.name} is missing the Obstacle script.");
        }

        // Register the obstacle with ObstacleManager
        obstacleManager.AddObstacle(obstacle);

        return obstacle;
    }

    // Finds a valid position for an obstacle on a specific tile
    private Vector3? FindValidPositionOnTile(Tile tile)
    {
        int maxRetries = 50;
        int obstacleLayerMask = 1 << LayerMask.NameToLayer("Obstacle");

        for (int attempt = 0; attempt < maxRetries; attempt++)
        {
            Vector3 randomPosition = GetRandomPositionOnTile(tile, out float obstacleRadius);
            if (IsPositionValid(randomPosition, obstacleRadius, obstacleLayerMask))
            {
                return randomPosition;
            }
        }

        return null; // No valid position found
    }

    // Generates a random position on the given tile within its safe bounds
    private Vector3 GetRandomPositionOnTile(Tile tile, out float obstacleRadius)
    {
        var tileSize = tile.TileSize;
        var tileCenter = tile.transform.position;

        // Create a temporary obstacle to calculate its radius
        GameObject tempObstacle = Instantiate(obstaclePrefab, tileCenter, Quaternion.identity);
        Obstacle obstacleScript = tempObstacle.GetComponent<Obstacle>();
        obstacleRadius = obstacleScript != null ? Mathf.Max(obstacleScript.ObstacleSize.x, obstacleScript.ObstacleSize.z) / 2 : 0;

        Destroy(tempObstacle); // Clean up temporary obstacle

        var (safeMinX, safeMaxX, safeMinZ, safeMaxZ) = CalculateSafeBounds(tileCenter, tileSize, obstacleRadius);

        float randomX = Random.Range(safeMinX, safeMaxX);
        float randomZ = Random.Range(safeMinZ, safeMaxZ);

        return new Vector3(randomX, tileCenter.y + tile.TileHeight / 2, randomZ);
    }

    // Calculates the safe spawn area for an obstacle
    private (float safeMinX, float safeMaxX, float safeMinZ, float safeMaxZ) CalculateSafeBounds(Vector3 tileCenter, Vector3 tileSize, float obstacleRadius)
    {
        float safeMinX = tileCenter.x - (tileSize.x / 2) + obstacleRadius;
        float safeMaxX = tileCenter.x + (tileSize.x / 2) - obstacleRadius;
        float safeMinZ = tileCenter.z - (tileSize.z / 2) + obstacleRadius;
        float safeMaxZ = tileCenter.z + (tileSize.z / 2) - obstacleRadius;

        return (safeMinX, safeMaxX, safeMinZ, safeMaxZ);
    }

    // Validates if a position is within bounds and free of overlaps
    private bool IsPositionValid(Vector3 position, float radius, int layerMask)
    {
        Collider[] colliders = Physics.OverlapSphere(position, radius, layerMask);
        return colliders.Length == 0;
    }
}
