using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TileConfig", menuName = "Settings/TileConfig")]
public class TileConfig : ScriptableObject
{
    [Header("Tile Prefab")]
    public GameObject TilePrefab;       // Prefab for the tile. Ensure this is assigned in the Inspector.

    [Header("Tile Size")]
    public Vector3 TileSize;           // Pre-calculated tile size (leave empty for automatic detection).
    public Vector3 DesiredSize = Vector3.one; // Desired uniform size for scaling tiles.

    [Header("Tile Variants")]
    public bool AllowVariants = false;  // Enable spawning multiple prefab variants.
    public List<GameObject> VariantPrefabs; // List of variant prefabs (if AllowVariants is true).
}
