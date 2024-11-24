using UnityEngine;

[CreateAssetMenu(fileName = "CameraSettings", menuName = "Settings/CameraSettings")]
public class CameraSettings : ScriptableObject
{
    [Header("Camera Behavior")]
    public Vector3 Offset = new Vector3(3, 12, -12);
    public float RotationAngle = 45f;
    public float FollowSpeed = 5f;
}