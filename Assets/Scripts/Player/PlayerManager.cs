// Scripts/Player/PlayerManager.cs
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject Player { get; private set; }
    public int Health { get; private set; } = 100;

    public void SetPlayer(GameObject player)
    {
        if (player == null)
        {
            Debug.LogError("Cannot set PlayerManager's player to null.");
            return;
        }

        Player = player;
        Debug.Log("Player has been set in PlayerManager.");
    }

    public void TakeDamage(int amount)
    {
        Health -= amount;
        if (Health <= 0)
        {
            HandlePlayerDeath();
        }
    }

    public void Heal(int amount)
    {
        Health = Mathf.Min(Health + amount, 100);
    }

    private void HandlePlayerDeath()
    {
        Debug.Log("Player has died!");
        // Add respawn logic or game over logic here
    }
}
