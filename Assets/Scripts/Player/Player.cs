using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Player : MonoBehaviour
{
    [SerializeField] public PlayerConfig config; // ScriptableObject for player settings

    public int CurrentHealth { get; private set; }
    public Tile CurrentTile { get; private set; }

    private float movementSpeed;

    void Start()
    {
        if (config == null)
        {
            Debug.LogError("PlayerConfig is not assigned!");
            enabled = false; // Disable script if config is missing
            return;
        }

        InitializePlayer();
    }

    private void InitializePlayer()
    {
        CurrentHealth = config.startingHealth;
        movementSpeed = config.moveSpeed;
        Debug.Log($"Player initialized with {CurrentHealth}/{config.maxHealth} health.");
    }

    public void UpdateCurrentTile(Tile newTile)
    {
        CurrentTile = newTile;
    }

    public void TakeDamage(int damage)
    {
        if (damage < 0) return; // Prevent healing via negative damage
        CurrentHealth -= damage;

        Debug.Log($"{name} took {damage} damage. Health is now {CurrentHealth}/{config.maxHealth}.");

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        if (amount < 0) return; // Prevent damage via negative healing
        CurrentHealth = Mathf.Min(CurrentHealth + amount, config.maxHealth);
        Debug.Log($"{name} healed {amount}. Health is now {CurrentHealth}/{config.maxHealth}.");
    }

    public void ModifyMovementSpeed(float multiplier)
    {
        movementSpeed = config.moveSpeed * multiplier;
    }

    public float GetMovementSpeed()
    {
        return movementSpeed;
    }

    private void Die()
    {
        Debug.Log($"{name} has died!");
        FindObjectOfType<PlayerManager>()?.HandlePlayerDeath();
        Destroy(gameObject); // Remove the player instance
    }
}
