using UnityEngine;

[CreateAssetMenu(fileName = "CameraSettings", menuName = "Settings/CameraSettings")]
public class CameraSettings : ScriptableObject
{
    [Header("Camera Offset and Angle")]
    public Vector3 offset = new Vector3(3, 12, -12);
    public float rotationAngle = 45f;

    [Header("Camera Follow Behavior")]
    public float followSpeed = 5f;

    // Future extensions: Add fields for advanced settings
    // e.g., dampening, transition effects, zoom, etc.
}
