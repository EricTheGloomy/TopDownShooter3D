using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private CameraSettings cameraSettings;
    [SerializeField] private Transform playerTransform;

    private Transform cameraTransform;

    void Start()
    {
        cameraTransform = Camera.main?.transform;

        if (cameraTransform == null)
        {
            Debug.LogError("Main Camera not found!");
            return;
        }

        if (cameraSettings == null)
        {
            Debug.LogError("CameraSettings ScriptableObject is not assigned.");
        }
    }

    void FixedUpdate()
    {
        if (playerTransform == null || cameraSettings == null || cameraTransform == null) return;

        FollowPlayer();
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
        if (cameraSettings == null || playerTransform == null || cameraTransform == null)
        {
            Debug.LogWarning("PositionCamera: Missing required references.");
            return;
        }

        cameraTransform.position = playerTransform.position + cameraSettings.Offset;
        cameraTransform.rotation = Quaternion.Euler(cameraSettings.RotationAngle, 0, 0);
    }

    private void FollowPlayer()
    {
        Vector3 targetPosition = playerTransform.position + cameraSettings.Offset;
        cameraTransform.position = Vector3.Lerp(
            cameraTransform.position, 
            targetPosition, 
            cameraSettings.FollowSpeed * Time.deltaTime
        );
    }
}


