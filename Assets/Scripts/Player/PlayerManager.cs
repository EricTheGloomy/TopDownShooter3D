// Scripts/Player/PlayerManager.cs
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject Player { get; private set; }

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

    public void HandlePlayerDeath()
    {
        Debug.Log("Player has died! Handle game-over or respawn logic here.");
        // Add game-over screen, respawn logic, etc.
    }
}
