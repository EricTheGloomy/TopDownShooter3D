using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private List<GameObject> enemies = new List<GameObject>();
    private Transform playerTransform; // Reference to the player


    public void SetPlayerTransform(Transform player)
    {
        if (player == null)
        {
            Debug.LogWarning("Player Transform is null. Enemies will not have a target.");
            return;
        }

        playerTransform = player;
        foreach (var enemy in enemies)
        {
            AssignPlayerToEnemy(enemy);
        }
    }

    public void AddEnemy(GameObject enemy)
    {
        if (enemy == null)
        {
            Debug.LogWarning("Cannot add a null enemy.");
            return;
        }

        if (!enemies.Contains(enemy))
        {
            enemies.Add(enemy);
            AssignPlayerToEnemy(enemy);
        }
    }

    public void RemoveEnemy(GameObject enemy)
    {
        if (enemy == null)
        {
            Debug.LogWarning("Cannot remove a null enemy.");
            return;
        }

        if (!enemies.Contains(enemy))
        {
            Debug.LogWarning("The enemy is not managed by this EnemyManager.");
            return;
        }

        enemies.Remove(enemy);
    }

    public IReadOnlyList<GameObject> GetAllEnemies()
    {
        return enemies.AsReadOnly();
    }

    public void DestroyAllEnemies()
    {
        foreach (var enemy in enemies)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }

        enemies.Clear();
    }

    private void AssignPlayerToEnemy(GameObject enemy)
    {
        var enemyController = enemy.GetComponent<EnemyController>();
        if (enemyController == null)
        {
            Debug.LogWarning($"Enemy {enemy.name} does not have an EnemyController component.");
            return;
        }

        if (playerTransform != null)
        {
            enemyController.SetPlayerTransform(playerTransform);
        }
    }
}
