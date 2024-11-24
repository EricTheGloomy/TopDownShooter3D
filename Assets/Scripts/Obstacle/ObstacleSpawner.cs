using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private ObstacleConfig config;

    private Map map;
    private readonly Dictionary<Tile, List<GameObject>> tileObstacles = new();

    public void Initialize(Map mapReference)
    {
        if (mapReference == null || config == null || config.DefaultObstaclePrefab == null)
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
        foreach (var tile in map.GetAllTiles())
        {
            if (!tileObstacles.ContainsKey(tile))
            {
                tileObstacles[tile] = new List<GameObject>();
            }

            for (int i = 0; i < config.ObstaclesPerTile; i++)
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
    }

    private GameObject InstantiateAndRegisterObstacle(Vector3 position, int index, Tile tile)
    {
        GameObject obstacle = Instantiate(config.DefaultObstaclePrefab, position, Quaternion.identity);

        AdjustTransformForTile(obstacle.transform, tile.transform);

        obstacle.name = $"Obstacle_{index}";

        Obstacle obstacleScript = obstacle.GetComponent<Obstacle>();
        if (obstacleScript != null)
        {
            // Adjust the Y position to align the bottom of the obstacle with the tile surface
            position.y += obstacleScript.ObstacleHeight / 2;
            obstacle.transform.position = position;
            obstacle.layer = LayerMask.NameToLayer(config.ObstacleLayer);
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
        Vector3 localScale = config.DefaultObstaclePrefab.transform.localScale;
        localScale.x /= tileTransform.localScale.x;
        localScale.y /= tileTransform.localScale.y;
        localScale.z /= tileTransform.localScale.z;
        obstacleTransform.localScale = localScale;
    }

    private Vector3? FindValidPositionOnTile(Tile tile)
    {
        for (int attempt = 0; attempt < config.MaxRetries; attempt++)
        {
            Vector3 position = tile.GetRandomPosition(config.DefaultObstacleRadius);
            if (IsPositionValid(position, config.DefaultObstacleRadius))
            {
                return position;
            }
        }
        return null;
    }

    private bool IsPositionValid(Vector3 position, float radius)
    {
        int obstacleLayer = 1 << LayerMask.NameToLayer(config.ObstacleLayer);
        return Physics.OverlapSphere(position, radius, obstacleLayer).Length == 0;
    }
}
