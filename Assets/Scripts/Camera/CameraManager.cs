using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CameraSettings cameraSettings; // Reference to the settings
    [SerializeField] private Transform playerTransform;     // Player Transform (assigned at runtime)

    private Transform cameraTransform;

    void Start()
    {
        cameraTransform = Camera.main.transform;

        if (cameraSettings == null)
        {
            Debug.LogError("CameraSettings ScriptableObject is not assigned.");
        }
    }

    void FixedUpdate()
    {
        if (playerTransform != null)
        {
            FollowPlayer();
        }
    }

    public void SetPlayer(Transform player)
    {
        if (player == null)
        {
            Debug.LogError("Player Transform is null when passed to CameraManager.");
            return;
        }

        playerTransform = player;
        PositionCamera();
    }

    private void PositionCamera()
    {
        if (playerTransform == null || cameraTransform == null || cameraSettings == null)
        {
            Debug.LogWarning("PositionCamera: Missing required references or settings.");
            return;
        }

        Vector3 desiredPosition = playerTransform.position + cameraSettings.offset;
        cameraTransform.position = desiredPosition;

        cameraTransform.rotation = Quaternion.Euler(cameraSettings.rotationAngle, 0, 0);
    }

    private void FollowPlayer()
    {
        if (playerTransform == null || cameraTransform == null || cameraSettings == null) return;

        Vector3 desiredPosition = playerTransform.position + cameraSettings.offset;
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, desiredPosition, cameraSettings.followSpeed * Time.deltaTime);

        cameraTransform.rotation = Quaternion.Euler(cameraSettings.rotationAngle, 0, 0);
    }
}
