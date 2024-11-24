using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyConfig config;

    private int currentHealth;
    private float currentSpeed;

    private void Start()
    {
        if (config == null)
        {
            Debug.LogError($"EnemyConfig is not assigned to {name}.");
            return;
        }

        InitializeStats();
    }

    private void InitializeStats()
    {
        currentHealth = config.Health;
        currentSpeed = config.MovementSpeed;
    }

    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }

    public void ModifySpeed(float multiplier)
    {
        currentSpeed = config.MovementSpeed * multiplier;
    }

    public void TakeDamage(int damage)
    {
        if (config == null) return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{name} has been defeated!");

        // Notify manager indirectly; decoupled responsibility.
        EnemyManager.Instance?.RemoveEnemy(gameObject);

        Destroy(gameObject);
    }
}
