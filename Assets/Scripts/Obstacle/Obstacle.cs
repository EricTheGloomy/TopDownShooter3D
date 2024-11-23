// Scripts/Obstacle/Obstacle.cs
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public Vector3 ObstacleSize { get; private set; }
    public float ObstacleHeight { get; private set; }

    private void Awake()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            ObstacleSize = renderer.bounds.size;
            ObstacleHeight = ObstacleSize.y;
        }
        else
        {
            Debug.LogError($"Obstacle {name} is missing a Renderer component.");
            ObstacleSize = Vector3.zero;
            ObstacleHeight = 0f;
        }
    }

    private void OnDestroy()
    {
        // Notify ObstacleManager
        ObstacleManager obstacleManager = FindObjectOfType<ObstacleManager>();
        if (obstacleManager != null)
        {
            obstacleManager.RemoveObstacle(gameObject);
        }
    }
}
