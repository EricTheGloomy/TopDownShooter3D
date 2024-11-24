using UnityEngine;

[CreateAssetMenu(fileName = "EnemyControllerSettings", menuName = "Settings/EnemyControllerSettings")]
public class EnemyControllerSettings : ScriptableObject
{
    [Header("Agent Movement")]
    public float TargetUpdateInterval = 0.2f;   // How often the agent recalculates pathfinding.
    public float StoppingDistance = 0.5f;      // Distance at which the agent stops approaching the target.
    public float Acceleration = 8f;            // Acceleration for the NavMeshAgent.
    public float MaxSpeedMultiplier = 1.5f;    // Multiplier for speed boosts or slowdowns.

    [Header("Obstacle Avoidance")]
    public int ObstacleAvoidancePriority = 50; // Priority for navigating around obstacles.
}
