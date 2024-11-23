using UnityEngine;

[CreateAssetMenu(fileName = "EnemyControllerSettings", menuName = "Settings/EnemyControllerSettings")]
public class EnemyControllerSettings : ScriptableObject
{
    public float targetUpdateInterval = 0.2f;
    public float stoppingDistance = 0.5f;
    public float acceleration = 8f;
    public int obstacleAvoidancePriority = 50;
}
