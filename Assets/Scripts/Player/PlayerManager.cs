// Scripts/Player/PlayerManager.cs
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject Player { get; private set; }
    private WeaponManager weaponManager;

    public void SetPlayer(GameObject player)
    {
        if (player == null)
        {
            Debug.LogError("Cannot set PlayerManager's player to null.");
            return;
        }

        Player = player;

        weaponManager = Player.GetComponent<WeaponManager>();
        if (weaponManager != null)
        {
            weaponManager.InitializeWeapons();
        }
        else
        {
            Debug.LogError("WeaponManager not found on Player.");
        }
    }

    public void HandlePlayerDeath()
    {
        Debug.Log("Player has died! Handle game-over or respawn logic here.");
    }
}
