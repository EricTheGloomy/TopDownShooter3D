using UnityEngine;

[CreateAssetMenu(fileName = "MapConfig", menuName = "Settings/MapConfig")]
public class MapConfig : ScriptableObject
{
    public string mapName;          // Optional name for the map
    public int gridWidth = 5;       // Number of tiles along the X-axis
    public int gridHeight = 5;      // Number of tiles along the Z-axis
    public TileConfig tileConfig;   // Reference to TileConfig for tile details
    public Color backgroundColor;   // Optional map-specific theme
}
