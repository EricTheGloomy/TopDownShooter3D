using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyConfig config; // Reference to ScriptableObject
    private int currentHealth;
    private float currentSpeed;

    private void Start()
    {
        if (config == null)
        {
            Debug.LogError($"EnemyConfig is not assigned to {name}.");
            return;
        }

        // Initialize current values from config
        currentHealth = config.health;
        currentSpeed = config.movementSpeed;
    }

    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }

    public void ModifySpeed(float multiplier)
    {
        // Update currentSpeed based on multiplier
        currentSpeed = config.movementSpeed * multiplier;
    }

    public void TakeDamage(int damage)
    {
        if (config == null) return; // Graceful handling for missing config

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{name} has been defeated!");

        // Notify EnemyManager (graceful handling for missing manager)
        var enemyManager = FindObjectOfType<EnemyManager>();
        if (enemyManager == null)
        {
            Debug.LogError("EnemyManager not found. Cannot remove enemy.");
        }
        else
        {
            enemyManager.RemoveEnemy(gameObject);
        }

        Destroy(gameObject); // Destroy the enemy object
    }
}
