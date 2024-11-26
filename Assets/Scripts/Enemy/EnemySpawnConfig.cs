// Scripts/Enemy/EnemySpawnConfig.cs
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpawnConfig", menuName = "Settings/EnemySpawnConfig")]
public class EnemySpawnConfig : ScriptableObject
{
    [System.Serializable]
    public class SpawnPattern
    {
        public string PatternName;                     // Identifier for the pattern
        [Range(0, 100)] public int Percentage;         // Percentage of tiles using this pattern
        public List<GameObject> EnemiesToSpawn;        // Enemy prefabs to spawn
        public List<int> EnemyCounts;                 // Number of each enemy to spawn (matches EnemiesToSpawn)
    }

    [Header("Spawn Patterns")]
    public List<SpawnPattern> Patterns;               // List of spawn patterns
}
