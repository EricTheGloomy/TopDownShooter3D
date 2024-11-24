// Scripts/Obstacle/ObstacleManager.cs
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    private readonly List<GameObject> obstacles = new();

    public void AddObstacle(GameObject obstacle)
    {
        if (obstacle == null)
        {
            Debug.LogWarning("Attempted to add a null obstacle.");
            return;
        }

        if (!obstacles.Contains(obstacle))
        {
            obstacles.Add(obstacle);
        }
    }

    public void RemoveObstacle(GameObject obstacle)
    {
        if (obstacle == null)
        {
            Debug.LogWarning("Attempted to remove a null obstacle.");
            return;
        }

        if (obstacles.Contains(obstacle))
        {
            obstacles.Remove(obstacle);
        }
    }

    public IReadOnlyList<GameObject> GetAllObstacles()
    {
        return obstacles.AsReadOnly();
    }

    public void DestroyAllObstacles()
    {
        foreach (var obstacle in obstacles)
        {
            Destroy(obstacle);
        }
        obstacles.Clear();
    }
}
