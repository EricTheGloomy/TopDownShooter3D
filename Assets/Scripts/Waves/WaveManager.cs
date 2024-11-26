// Scripts/Wave/WaveManager.cs
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private List<WaveConfig> waves;       // List of wave configurations
    [SerializeField] private EnemySpawner enemySpawner;    // Reference to the EnemySpawner
    private int currentWaveIndex = 0;                      // Index of the current wave
    private bool isWaveActive = false;                     // Whether the wave is currently active

    private void Start()
    {
        StartCoroutine(WaveLoop());
    }

    private IEnumerator WaveLoop()
    {
         // Wait until the GameManager initializes EnemySpawner
        yield return new WaitUntil(() => enemySpawner != null && enemySpawner.GetMap() != null);

        while (currentWaveIndex < waves.Count)
        {
            var wave = waves[currentWaveIndex];
            Debug.Log($"Starting Wave {currentWaveIndex + 1}");
            yield return StartCoroutine(SpawnWave(wave));

            Debug.Log($"Wave {currentWaveIndex + 1} Complete");
            currentWaveIndex++;

            if (currentWaveIndex < waves.Count)
            {
                Debug.Log($"Next wave starts in {wave.WaveDelay} seconds...");
                yield return new WaitForSeconds(wave.WaveDelay);
            }
        }

        Debug.Log("All waves complete!");
    }

    private IEnumerator SpawnWave(WaveConfig wave)
    {
        isWaveActive = true;

        int enemiesSpawned = 0;

        // Repeat patterns until the wave's enemy cap is reached
        while (enemiesSpawned < wave.NumberOfEnemies)
        {
            foreach (var pattern in wave.Patterns)
            {
                if (enemiesSpawned >= wave.NumberOfEnemies) break;

                int enemiesToSpawnInPattern = Mathf.Min(
                    pattern.EnemiesToSpawn.Count * pattern.EnemyCounts.Sum(),
                    wave.NumberOfEnemies - enemiesSpawned
                );

                yield return StartCoroutine(SpawnPattern(pattern, enemiesToSpawnInPattern, wave.SpawnDelay));
                enemiesSpawned += enemiesToSpawnInPattern;
            }
        }

        isWaveActive = false;
    }

    private IEnumerator SpawnPattern(EnemySpawnConfig.SpawnPattern pattern, int totalEnemies, float spawnDelay)
    {
        List<Tile> availableTiles = new List<Tile>(enemySpawner.GetAvailableTiles());
        int enemiesSpawned = 0;

        while (enemiesSpawned < totalEnemies && availableTiles.Count > 0)
        {
            Tile tile = availableTiles[Random.Range(0, availableTiles.Count)];
            availableTiles.Remove(tile);

            for (int i = 0; i < pattern.EnemiesToSpawn.Count; i++)
            {
                for (int count = 0; count < pattern.EnemyCounts[i]; count++)
                {
                    if (enemiesSpawned >= totalEnemies) break;

                    enemySpawner.SpawnEnemyOnTile(tile, pattern.EnemiesToSpawn[i]);
                    enemiesSpawned++;

                    yield return new WaitForSeconds(spawnDelay);
                }
            }
        }
    }
}
