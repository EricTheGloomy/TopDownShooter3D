using UnityEngine;

[CreateAssetMenu(fileName = "MapConfig", menuName = "Settings/MapConfig")]
public class MapConfig : ScriptableObject
{
    [Header("Map Dimensions")]
    public int GridWidth = 5;       // Number of tiles along the X-axis.
    public int GridHeight = 5;      // Number of tiles along the Z-axis.

    [Header("Tile Configuration")]
    public TileConfig TileConfig;   // Reference to TileConfig for prefab and size.

    [Header("Map Theme")]
    public string MapName;          // Optional name for the map.
    public Color BackgroundColor;   // Background color for the map.
}
