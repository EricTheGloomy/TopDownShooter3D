using UnityEngine;

[CreateAssetMenu(fileName = "TileConfig", menuName = "Settings/TileConfig")]
public class TileConfig : ScriptableObject
{
    public GameObject tilePrefab;     // Tile prefab
    public Vector3 tileSize;          // Pre-calculated tile size
    public Vector3 desiredSize = Vector3.one; // Desired uniform size for tiles
}
