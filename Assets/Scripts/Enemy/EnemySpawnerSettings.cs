using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpawnerSettings", menuName = "Settings/EnemySpawnerSettings")]
public class EnemySpawnerSettings : ScriptableObject
{
    [Header("Spawning Configuration")]
    public int EnemiesToSpawn = 10;        // Total number of enemies to spawn.
    public float EnemyRadius = 0.25f;      // Radius used to ensure no overlap during spawn.
    public int MaxRetries = 50;            // Maximum attempts to find valid spawn positions.

    [Header("Spawn Behavior")]
    public int MinObstaclePriority = 20;   // Minimum priority for obstacle avoidance.
    public int MaxObstaclePriority = 80;   // Maximum priority for obstacle avoidance.
    public bool RandomizeSpawnOrder = true; // Whether to shuffle spawn order across tiles.
}
