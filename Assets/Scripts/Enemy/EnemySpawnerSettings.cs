using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpawnerSettings", menuName = "Settings/EnemySpawnerSettings")]
public class EnemySpawnerSettings : ScriptableObject
{
    [Header("Spawning Configuration")]
    public int enemiesToSpawn = 10;
    public float enemyRadius = 0.25f; // Size used for overlap check
    public int maxRetries = 50; // Max attempts to find valid positions

    [Header("Obstacle Avoidance Priority")]
    public int minObstaclePriority = 20;
    public int maxObstaclePriority = 80;
}
