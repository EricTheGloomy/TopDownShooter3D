// Scripts/Map/Tile.cs
using UnityEngine;

public class Tile : MonoBehaviour
{
    // Store the tile size for external use
    public Vector3 TileSize { get; private set; }
    public float TileHeight { get; private set; } // Store the height of the tile

    private void Awake()
    {
        // Automatically calculate and store the tile size when the script is first loaded
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            TileSize = renderer.bounds.size;
            TileHeight = TileSize.y; // The height of the tile is the Y size
        }
        else
        {
            Debug.LogError($"Tile {name} is missing a Renderer component.");
            TileSize = Vector3.zero;
            TileHeight = 0f;
        }
    }
}
