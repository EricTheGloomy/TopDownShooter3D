// Scripts/Player/Player.cs
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Player : MonoBehaviour
{
    [SerializeField] public PlayerConfig config; // ScriptableObject for player settings

    public int Health { get; private set; }
    public Tile CurrentTile { get; private set; }

    private float currentSpeed;

    void Start()
    {
        if (config == null)
        {
            Debug.LogError("PlayerConfig is not assigned!");
            return;
        }

        InitializePlayer();
    }

    private void InitializePlayer()
    {
        Health = config.startingHealth;
        currentSpeed = config.moveSpeed;
    }

    public void UpdateCurrentTile(Tile newTile)
    {
        CurrentTile = newTile;
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;

        if (Health <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        Health = Mathf.Min(Health + amount, config.maxHealth);
    }

    public void ModifySpeed(float multiplier)
    {
        currentSpeed = config.moveSpeed * multiplier;
    }

    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }

    private void Die()
    {
        Debug.Log($"{name} has died!");
        // Notify the PlayerManager about the death (optional extension)
        FindObjectOfType<PlayerManager>()?.HandlePlayerDeath();
        Destroy(gameObject); // Remove the player instance
    }
}
