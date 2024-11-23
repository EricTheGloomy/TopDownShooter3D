// Scripts/Map/MapSpawner.cs
using UnityEngine;

public class MapSpawner : MonoBehaviour
{
    public GameObject mapTilePrefab; // Assign the MapTile prefab in the Inspector
    public GameObject mapContainer; // Assign MapContainer in the Inspector, or it will create one

    public int gridWidth = 5; // Number of tiles along the X-axis
    public int gridHeight = 5; // Number of tiles along the Z-axis

    public void PopulateMap(Map map)
    {
        // Ensure MapContainer exists
        if (mapContainer == null)
        {
            mapContainer = new GameObject("MapContainer");
        }

        // Get tile size by instantiating a temporary tile
        GameObject tempTile = Instantiate(mapTilePrefab);
        Tile tempTileScript = tempTile.GetComponent<Tile>();
        if (tempTileScript == null)
        {
            tempTileScript = tempTile.AddComponent<Tile>();
        }
        Vector3 tileSize = tempTileScript.TileSize;
        Destroy(tempTile); // Clean up temporary tile

        Debug.Log($"Tile size detected: {tileSize}");

        // Use the tile size to calculate the spacing
        float tileWidth = tileSize.x;
        float tileDepth = tileSize.z;

        // Generate the grid
        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                Vector3 position = new Vector3(x * tileWidth, 0, z * tileDepth);
                GameObject tileObject = SpawnTile(position);

                // Add the tile to the map
                Tile tile = tileObject.GetComponent<Tile>();
                map.AddTile(tile, x, z);
            }
        }
    }

    // Spawns a single tile at the given position
    private GameObject SpawnTile(Vector3 position)
    {
        if (mapTilePrefab == null)
        {
            Debug.LogError("MapTilePrefab is not assigned.");
            return null;
        }

        GameObject newTile = Instantiate(mapTilePrefab, position, Quaternion.identity);
        newTile.transform.SetParent(mapContainer.transform);
        newTile.name = $"Tile_{position.x}_{position.y}_{position.z}";
        return newTile;
    }
}
