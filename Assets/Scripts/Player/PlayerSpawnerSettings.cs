using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSpawnerSettings", menuName = "Settings/PlayerSpawnerSettings")]
public class PlayerSpawnerSettings : ScriptableObject
{
    public GameObject playerPrefab;      // Player prefab to spawn
    public int maxRetries = 50;          // Maximum retries to find a valid spawn point
    public float spawnRadius = 0.5f;     // Radius for overlap checks
    public string obstacleLayerName = "Obstacle"; // Layer name for obstacles
}
