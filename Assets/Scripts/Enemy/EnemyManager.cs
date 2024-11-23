using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private List<GameObject> enemies = new List<GameObject>();
    private Transform playerTransform; // Reference to the player

    public void SetPlayerTransform(Transform player)
    {
        playerTransform = player;

        // Assign the player transform to all existing enemies
        foreach (var enemy in enemies)
        {
            var enemyController = enemy.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.SetPlayerTransform(playerTransform);
            }
        }
    }

    public void AddEnemy(GameObject enemy)
    {
        if (!enemies.Contains(enemy))
        {
            enemies.Add(enemy);

            // Assign the player transform to the new enemy
            var enemyController = enemy.GetComponent<EnemyController>();
            if (enemyController != null && playerTransform != null)
            {
                enemyController.SetPlayerTransform(playerTransform);
            }
        }
    }

    public void RemoveEnemy(GameObject enemy)
    {
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
        }
    }

    public List<GameObject> GetAllEnemies()
    {
        return new List<GameObject>(enemies); // Return a copy
    }

    public void DestroyAllEnemies()
    {
        foreach (var enemy in enemies)
        {
            Destroy(enemy);
        }
        enemies.Clear();
    }
}
