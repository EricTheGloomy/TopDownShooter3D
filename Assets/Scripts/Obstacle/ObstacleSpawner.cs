// Scripts/Obstacle/ObstacleSpawner.cs
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private ObstacleConfig config;

    private Map map;
    private readonly Dictionary<Tile, List<GameObject>> tileObstacles = new();
    private ObstacleManager obstacleManager;

    private float cachedObstacleRadius = -1f;

    public void Initialize(Map mapReference, ObstacleManager obstacleManagerReference)
    {
        if (mapReference == null || obstacleManagerReference == null || obstaclePrefab == null)
        {
            Debug.LogError("ObstacleSpawner dependencies are not properly assigned.");
            return;
        }

        map = mapReference;
        obstacleManager = obstacleManagerReference;

        SpawnObstacles();
        Debug.Log("ObstacleSpawner initialized.");
    }

    private void SpawnObstacles()
    {
        foreach (var tile in map.GetAllTiles())
        {
            if (!tileObstacles.ContainsKey(tile))
            {
                tileObstacles[tile] = new List<GameObject>();
            }

            for (int i = 0; i < config.obstaclesPerTile; i++)
            {
                var validPosition = FindValidPositionOnTile(tile);
                if (validPosition.HasValue)
                {
                    var obstacle = InstantiateAndRegisterObstacle(validPosition.Value, tileObstacles[tile].Count + 1, tile);
                    tileObstacles[tile].Add(obstacle);
                }
                else
                {
                    Debug.LogWarning($"Could not find a valid position for an obstacle on tile {tile.name} after {config.maxRetries} retries.");
                }
            }
        }

        Debug.Log($"Obstacles spawned for {tileObstacles.Count} tiles.");
    }

    private GameObject InstantiateAndRegisterObstacle(Vector3 position, int index, Tile tile)
    {
        var obstacle = Instantiate(obstaclePrefab, position, Quaternion.identity);

        AdjustTransformForTile(obstacle.transform, tile.transform);

        obstacle.name = $"Obstacle_{index}";

        var obstacleScript = obstacle.GetComponent<Obstacle>();
        if (obstacleScript != null)
        {
            position.y += obstacleScript.ObstacleHeight / 2;
            obstacle.transform.position = position;
            obstacle.layer = LayerMask.NameToLayer(config.obstacleLayer);
        }
        else
        {
            Debug.LogError($"Obstacle {obstacle.name} is missing the Obstacle script.");
        }

        obstacleManager.AddObstacle(obstacle);

        return obstacle;
    }

    private void AdjustTransformForTile(Transform obstacleTransform, Transform tileTransform)
    {
        obstacleTransform.SetParent(tileTransform, true);
        Vector3 localScale = obstaclePrefab.transform.localScale;
        localScale.x /= tileTransform.localScale.x;
        localScale.y /= tileTransform.localScale.y;
        localScale.z /= tileTransform.localScale.z;
        obstacleTransform.localScale = localScale;
    }

    private Vector3? FindValidPositionOnTile(Tile tile)
    {
        float radius = GetObstacleRadius();
        for (int attempt = 0; attempt < config.maxRetries; attempt++)
        {
            var position = tile.GetRandomPosition(radius);
            if (IsPositionValid(position, radius))
            {
                return position;
            }
        }

        return null;
    }

    private float GetObstacleRadius()
    {
        if (cachedObstacleRadius < 0f)
        {
            var tempObstacle = Instantiate(obstaclePrefab);
            var obstacleScript = tempObstacle.GetComponent<Obstacle>();
            cachedObstacleRadius = obstacleScript != null
                ? Mathf.Max(obstacleScript.ObstacleSize.x, obstacleScript.ObstacleSize.z) / 2
                : config.defaultObstacleRadius;
            Destroy(tempObstacle);
        }
        return cachedObstacleRadius;
    }

    private bool IsPositionValid(Vector3 position, float radius)
    {
        int layerMask = 1 << LayerMask.NameToLayer(config.obstacleLayer);
        return Physics.OverlapSphere(position, radius, layerMask).Length == 0;
    }
}
