using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    private static ObstacleManager instance;
    public static ObstacleManager Instance => instance;

    private readonly List<GameObject> obstacles = new();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;
    }

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

        obstacles.Remove(obstacle);
    }

    public IReadOnlyList<GameObject> GetAllObstacles()
    {
        return obstacles.AsReadOnly();
    }

    public void DestroyAllObstacles()
    {
        foreach (var obstacle in obstacles)
        {
            if (obstacle != null) Destroy(obstacle);
        }
        obstacles.Clear();
    }
}
