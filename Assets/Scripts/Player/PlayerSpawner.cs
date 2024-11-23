// Scripts/Player/PlayerSpawner.cs
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab; // Assign the Player prefab in the Inspector
    private Map map; // Reference to the Map

    public void Initialize(Map mapReference)
    {
        Debug.Log("Initializing PlayerSpawner...");
        map = mapReference;

        if (map == null)
        {
            Debug.LogError("Map reference is null. PlayerSpawner cannot initialize.");
            return;
        }

        SpawnPlayer();

        Debug.Log("PlayerSpawner initialized.");
    }

    private void SpawnPlayer()
    {
        if (playerPrefab == null)
        {
            Debug.LogError("PlayerPrefab is not assigned.");
            return;
        }

        // Try to find a valid spawn position
        Vector3? spawnPosition = FindValidSpawnPosition();
        if (spawnPosition.HasValue)
        {
            GameObject player = Instantiate(playerPrefab, spawnPosition.Value, Quaternion.identity);
            player.name = "Player";

            // Let the PlayerManager handle the player instance
            FindObjectOfType<PlayerManager>()?.SetPlayer(player);
        }
        else
        {
            Debug.LogError("Failed to find a valid spawn position for the player.");
        }
    }

    private Vector3? FindValidSpawnPosition()
    {
        int maxAttempts = 50; // Limit retries to avoid infinite loops
        float playerRadius = 0.5f; // Radius for overlap check, adjust as needed
        int obstacleLayerMask = 1 << LayerMask.NameToLayer("Obstacle");

        // Get all tiles and shuffle them to ensure random order
        List<Tile> tiles = new List<Tile>(map.GetAllTiles());
        ShuffleTiles(tiles);

        foreach (var tile in tiles)
        {
            for (int attempt = 0; attempt < maxAttempts; attempt++)
            {
                // Randomize a position on the tile
                Vector3 tileCenter = tile.transform.position;
                Vector3 tileSize = tile.TileSize;

                float randomX = Random.Range(tileCenter.x - tileSize.x / 2, tileCenter.x + tileSize.x / 2);
                float randomZ = Random.Range(tileCenter.z - tileSize.z / 2, tileCenter.z + tileSize.z / 2);
                Vector3 spawnPosition = new Vector3(randomX, tileCenter.y + 1, randomZ); // Spawn slightly above the tile

                // Check if the position is free of obstacles
                if (Physics.OverlapSphere(spawnPosition, playerRadius, obstacleLayerMask).Length == 0)
                {
                    return spawnPosition;
                }
            }
        }

        // If no valid position is found, return null
        return null;
    }

    // Utility method to shuffle the list of tiles
    private void ShuffleTiles(List<Tile> tiles)
    {
        for (int i = tiles.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            Tile temp = tiles[i];
            tiles[i] = tiles[randomIndex];
            tiles[randomIndex] = temp;
        }
    }
}
