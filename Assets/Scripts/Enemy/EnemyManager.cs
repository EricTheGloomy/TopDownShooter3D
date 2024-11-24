using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager instance;
    public static EnemyManager Instance => instance;

    private readonly List<GameObject> enemies = new List<GameObject>();
    private Transform playerTransform;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;
    }

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
        if (enemy == null || enemies.Contains(enemy)) return;

        enemies.Add(enemy);
        AssignPlayerToEnemy(enemy);
    }

    public void RemoveEnemy(GameObject enemy)
    {
        if (enemy == null || !enemies.Contains(enemy)) return;

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
            if (enemy != null) Destroy(enemy);
        }

        enemies.Clear();
    }

    private void AssignPlayerToEnemy(GameObject enemy)
    {
        var controller = enemy.GetComponent<EnemyController>();
        if (controller == null) return;

        controller.SetPlayerTransform(playerTransform);
    }
}
