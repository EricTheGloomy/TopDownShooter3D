// Scripts/Obstacle/ObstacleManager.cs
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    private List<GameObject> obstacles = new List<GameObject>();

    public void AddObstacle(GameObject obstacle)
    {
        if (!obstacles.Contains(obstacle))
        {
            obstacles.Add(obstacle);
        }
    }

    public void RemoveObstacle(GameObject obstacle)
    {
        if (obstacles.Contains(obstacle))
        {
            obstacles.Remove(obstacle);
        }
    }

    public List<GameObject> GetAllObstacles()
    {
        return new List<GameObject>(obstacles); // Return a copy for safety
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
