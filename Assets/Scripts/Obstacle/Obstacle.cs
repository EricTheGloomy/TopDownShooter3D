// Scripts/Obstacle/Obstacle.cs
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public Vector3 ObstacleSize { get; private set; }
    public float ObstacleHeight { get; private set; }

    private void Awake()
    {
        var renderer = GetComponent<Renderer>();
        if (renderer == null)
        {
            Debug.LogError($"Obstacle {name} is missing a Renderer component.");
            ObstacleSize = Vector3.zero;
            ObstacleHeight = 0f;
            return;
        }

        ObstacleSize = renderer.bounds.size;
        ObstacleHeight = ObstacleSize.y;
    }

    private void OnDestroy()
    {
        var obstacleManager = FindObjectOfType<ObstacleManager>();
        if (obstacleManager != null)
        {
            obstacleManager.RemoveObstacle(gameObject);
        }
    }
}
