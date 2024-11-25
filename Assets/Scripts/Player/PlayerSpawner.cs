using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private PlayerSpawnerSettings spawnerSettings; // Updated settings
    private Map map;

    public void Initialize(Map mapReference)
    {
        if (mapReference == null)
        {
            Debug.LogError("Map reference is null. PlayerSpawner cannot initialize.");
            return;
        }

        if (spawnerSettings == null)
        {
            Debug.LogError("PlayerSpawnerSettings is not assigned!");
            return;
        }

        if (spawnerSettings.playerPrefab == null)
        {
            Debug.LogError("Player prefab is not assigned in PlayerSpawnerSettings!");
            return;
        }

        map = mapReference;
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        Vector3? spawnPosition = FindValidSpawnPosition();

        if (spawnPosition.HasValue)
        {
            GameObject player = Instantiate(spawnerSettings.playerPrefab, spawnPosition.Value, Quaternion.identity);
            player.name = "Player";

            var playerManager = FindObjectOfType<PlayerManager>();
            if (playerManager != null)
            {
                playerManager.SetPlayer(player);
            }
            else
            {
                Debug.LogError("PlayerManager not found in the scene.");
            }
        }
        else
        {
            Debug.LogError("Failed to find a valid spawn position for the player.");
        }
    }

    private Vector3? FindValidSpawnPosition()
    {
        int maxAttempts = spawnerSettings.maxRetries;
        float playerRadius = spawnerSettings.spawnRadius;
        int obstacleLayerMask = 1 << LayerMask.NameToLayer(spawnerSettings.obstacleLayerName);

        List<Tile> tiles = new List<Tile>(map.GetAllTiles());
        ShuffleTiles(tiles);

        foreach (var tile in tiles)
        {
            for (int attempt = 0; attempt < maxAttempts; attempt++)
            {
                Vector3 tileCenter = tile.transform.position;
                Vector3 tileSize = tile.TileSize;

                float randomX = Random.Range(tileCenter.x - tileSize.x / 2, tileCenter.x + tileSize.x / 2);
                float randomZ = Random.Range(tileCenter.z - tileSize.z / 2, tileCenter.z + tileSize.z / 2);
                Vector3 spawnPosition = new Vector3(randomX, tileCenter.y + 1, randomZ);

                if (Physics.OverlapSphere(spawnPosition, playerRadius, obstacleLayerMask).Length == 0)
                {
                    return spawnPosition;
                }
            }
        }

        return null;
    }

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
