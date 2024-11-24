using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public int Width { get; private set; }  // Map width in tiles
    public int Height { get; private set; } // Map height in tiles

    private Tile[,] tiles; // 2D array for efficient grid representation
    private readonly Dictionary<Vector3, Tile> tileDictionary = new Dictionary<Vector3, Tile>();

    // Initialize the map with dimensions
    public void Initialize(int width, int height)
    {
        Width = width;
        Height = height;
        tiles = new Tile[width, height];
    }

    // Add a tile to the map
    public bool AddTile(Tile tile, int x, int z)
    {
        if (x < 0 || x >= Width || z < 0 || z >= Height)
        {
            Debug.LogError($"Tile at ({x}, {z}) is out of bounds!");
            return false;
        }

        Vector3 position = tile.transform.position;

        // Add to the 2D array and dictionary
        tiles[x, z] = tile;
        tileDictionary[position] = tile;
        return true;
    }

    // Get a tile at a specific grid position
    public Tile GetTileAt(int x, int z)
    {
        if (x < 0 || x >= Width || z < 0 || z >= Height)
        {
            Debug.LogWarning($"Requested tile at ({x}, {z}) is out of bounds.");
            return null;
        }
        return tiles[x, z];
    }

    // Get a tile at a specific world position
    public Tile GetTileAtPosition(Vector3 position)
    {
        tileDictionary.TryGetValue(position, out Tile tile);
        return tile;
    }

    // Get all tiles in the map
    public IEnumerable<Tile> GetAllTiles()
    {
        foreach (var tile in tileDictionary.Values)
        {
            yield return tile;
        }
    }
}
