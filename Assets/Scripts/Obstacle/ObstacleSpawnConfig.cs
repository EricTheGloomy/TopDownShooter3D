// Scripts/Obstacle/ObstacleSpawnConfig.cs
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObstacleSpawnConfig", menuName = "Settings/ObstacleSpawnConfig")]
public class ObstacleSpawnConfig : ScriptableObject
{
    [System.Serializable]
    public class SpawnPattern
    {
        public string PatternName;                      // Identifier for the pattern
        [Range(0, 100)] public int Percentage;          // Percentage of tiles using this pattern
        public List<GameObject> ObstaclesToSpawn;       // Obstacles to spawn for this pattern
        public List<int> ObstacleCounts;                // Number of each obstacle to spawn (matches ObstaclesToSpawn)
    }

    [Header("Spawn Patterns")]
    public List<SpawnPattern> Patterns;                // List of spawn patterns
}
