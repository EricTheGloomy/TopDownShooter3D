using UnityEngine;

public class MapSpawner : MonoBehaviour
{
    public TileConfig tileConfig;
    public GameObject mapContainer;

    public void PopulateMap(Map map, TileConfig tileConfig)
    {
        if (tileConfig == null || tileConfig.tilePrefab == null)
        {
            throw new MissingReferenceException("TileConfig or tilePrefab is not assigned in MapSpawner.");
        }

        if (mapContainer == null)
        {
            mapContainer = new GameObject("MapContainer");
        }

        Vector3 tileSize = tileConfig.tileSize;

        if (tileSize == Vector3.zero)
        {
            tileSize = CalculateTileSize(tileConfig.tilePrefab);
        }

        for (int x = 0; x < map.Width; x++)
        {
            for (int z = 0; z < map.Height; z++)
            {
                Vector3 position = new Vector3(x * tileConfig.desiredSize.x, 0, z * tileConfig.desiredSize.z);
                GameObject tileObject = SpawnTile(position);

                Tile tile = tileObject.GetComponent<Tile>();
                if (tile != null)
                {
                    tile.Initialize(tileConfig.desiredSize); // Pass desired size to Tile
                    map.AddTile(tile, x, z);
                }
            }
        }
    }

    private GameObject SpawnTile(Vector3 position)
    {
        GameObject newTile = Instantiate(tileConfig.tilePrefab, position, Quaternion.identity);
        newTile.transform.SetParent(mapContainer.transform);
        newTile.name = $"Tile_{position.x}_{position.y}_{position.z}";
        return newTile;
    }

    private Vector3 CalculateTileSize(GameObject tilePrefab)
    {
        GameObject tempTile = Instantiate(tilePrefab);
        Renderer renderer = tempTile.GetComponent<Renderer>();
        Vector3 size = renderer != null ? renderer.bounds.size : Vector3.one;
        Destroy(tempTile);

        Debug.Log($"Calculated tile size: {size}");
        return size;
    }
}
