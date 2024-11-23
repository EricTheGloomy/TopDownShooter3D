// Scripts/Camera/CameraManager.cs
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform playerTransform; // Reference to the player's transform
    [SerializeField] private Vector3 offset = new Vector3(3, 12, -12); // Default offset
    [SerializeField] private float rotationAngle = 45f; // Default angle for the camera
    [SerializeField] private float followSpeed = 5f; // Speed at which the camera follows the player

    private Transform cameraTransform; // Reference to the camera's transform

    void Start()
    {
        cameraTransform = Camera.main.transform;

        if (playerTransform == null)
        {
            Debug.LogWarning("Player Transform is not assigned. CameraManager will not follow the player.");
        }
        else
        {
            PositionCamera();
        }
    }

    void FixedUpdate()
    {
        if (playerTransform != null)
        {
            FollowPlayer();
        }
    }

    // Public method to set the player reference (called after player is spawned)
    public void SetPlayer(Transform player)
    {
        playerTransform = player;
        PositionCamera();
    }

    private void PositionCamera()
    {
        // Position the camera based on the player's position and the offset
        Vector3 desiredPosition = playerTransform.position + offset;
        cameraTransform.position = desiredPosition;

        // Set the rotation of the camera to look at the player from the specified angle
        cameraTransform.rotation = Quaternion.Euler(rotationAngle, 0, 0);
    }

    private void FollowPlayer()
    {
        // Smoothly interpolate to the desired position
        Vector3 desiredPosition = playerTransform.position + offset;
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, desiredPosition, followSpeed * Time.deltaTime);

        // Keep the rotation fixed
        cameraTransform.rotation = Quaternion.Euler(rotationAngle, 0, 0);
    }
}
