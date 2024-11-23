// Scripts/Player/Player.cs
using UnityEngine;

public class Player : MonoBehaviour
{
    public int Health { get; private set; } = 100;
    public Tile CurrentTile { get; private set; }

    public void TakeDamage(int amount)
    {
        Health -= amount;
        if (Health <= 0)
        {
            Die();
        }
    }

    public void UpdateCurrentTile(Tile newTile)
    {
        CurrentTile = newTile;
    }

    private void Die()
    {
        Debug.Log($"{name} has died!");
        // Handle death logic
    }
}
