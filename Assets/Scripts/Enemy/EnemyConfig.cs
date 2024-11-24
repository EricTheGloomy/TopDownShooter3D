using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfig", menuName = "Settings/EnemyConfig")]
public class EnemyConfig : ScriptableObject
{
    [Header("Health & Speed")]
    public int Health = 50;               // Default health value.
    public float MovementSpeed = 3f;      // Default speed value.

    [Header("Behavior Settings")]
    public bool Aggressive = true;        // Determines enemy AI behavior.
    public float DetectionRange = 10f;    // How far the enemy can detect the player.
}
