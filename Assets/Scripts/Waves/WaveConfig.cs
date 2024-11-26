// Scripts/Wave/WaveConfig.cs
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveConfig", menuName = "Settings/WaveConfig")]
public class WaveConfig : ScriptableObject
{
    [Header("Timing")]
    public float WaveDelay = 5f;       // Time before the wave starts
    public float SpawnDelay = 0.5f;    // Delay between individual spawns within the wave

    [Header("Patterns")]
    public List<EnemySpawnConfig.SpawnPattern> Patterns; // Patterns to use in this wave

    [Header("Scaling")]
    public int NumberOfEnemies = 10;  // Total number of enemies in this wave
}
