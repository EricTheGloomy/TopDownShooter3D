// Scripts/Enemy/EnemySpawnerSettings.cs
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpawnerSettings", menuName = "Settings/EnemySpawnerSettings")]
public class EnemySpawnerSettings : ScriptableObject
{
    [Header("Spawning Configuration")]
    public int MaxRetries = 50;          // Maximum retries to find valid spawn positions
    public float EnemyRadius = 0.5f;     // Radius for overlap checks

    [Header("Layer Settings")]
    public string obstacleLayerName = "Obstacle"; // Name of the obstacle layer for collision checks
}
