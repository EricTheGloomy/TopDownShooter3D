// Scripts/Obstacle/ObstacleConfig.cs
using UnityEngine;

[CreateAssetMenu(fileName = "ObstacleConfig", menuName = "Settings/ObstacleConfig")]
public class ObstacleConfig : ScriptableObject
{
    [Header("Obstacle Spawning")]
    public int obstaclesPerTile = 2;
    public int maxRetries = 50;
    public string obstacleLayer = "Obstacle";

    [Header("Fallback Settings")]
    public float defaultObstacleRadius = 0.5f; // Used if obstacle size can't be determined dynamically
}
