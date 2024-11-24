using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObstacleConfig", menuName = "Settings/ObstacleConfig")]
public class ObstacleConfig : ScriptableObject
{
    [Header("Obstacle Prefabs")]
    public GameObject DefaultObstaclePrefab;
    public bool AllowVariants = false;
    public List<GameObject> VariantPrefabs;

    [Header("Spawning Configuration")]
    public int ObstaclesPerTile = 2;
    public int MaxRetries = 50;
    public float DefaultObstacleRadius = 0.5f;

    [Header("Scaling")]
    public bool AllowScaling = true;
    public Vector3 MinScale = Vector3.one * 0.8f;
    public Vector3 MaxScale = Vector3.one * 1.2f;

    [Header("Layer Settings")]
    public string ObstacleLayer = "Obstacle"; // Name of the layer obstacles belong to.
}
