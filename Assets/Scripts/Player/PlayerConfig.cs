using UnityEngine;

[CreateAssetMenu(fileName = "PlayerConfig", menuName = "Settings/PlayerConfig")]
public class PlayerConfig : ScriptableObject
{
    [Header("Health")]
    public int startingHealth = 100;
    public int maxHealth = 100;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 180f;

    [Header("Other Attributes")]
    public float stamina = 100f; // Placeholder for stamina or future attributes
}
