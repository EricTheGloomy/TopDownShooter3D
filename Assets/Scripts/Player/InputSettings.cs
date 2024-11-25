using UnityEngine;

[CreateAssetMenu(fileName = "InputSettings", menuName = "Settings/InputSettings")]
public class InputSettings : ScriptableObject
{
    [Header("Movement Axes")]
    public string horizontalAxis = "Horizontal"; // Default A/D or Left/Right
    public string verticalAxis = "Vertical";     // Default W/S or Up/Down

    [Header("Actions")]
    public KeyCode attackKey = KeyCode.Space;    // Example: Dash
    public KeyCode interactKey = KeyCode.E;      // Example: Interaction
}
