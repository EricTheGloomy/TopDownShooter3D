using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfig", menuName = "Settings/EnemyConfig")]
public class EnemyConfig : ScriptableObject
{
    public int health = 50; // Default health
    public float movementSpeed = 3f; // Default speed
}
