using UnityEngine;

public class MapSpawner : MonoBehaviour
{
    [SerializeField] private TileConfig tileConfig;
    [SerializeField] private GameObject mapContainer;

    public void PopulateMap(Map map, TileConfig config)
    {
        if (config == null || config.TilePrefab == null)
        {
            Debug.LogError("TileConfig or TilePrefab is not assigned in MapSpawner.");
            return;
        }

        if (mapContainer == null)
        {
            mapContainer = new GameObject("MapContainer");
        }

        Vector3 tileSize = config.TileSize != Vector3.zero ? config.TileSize : CalculateTileSize(config.TilePrefab);

        for (int x = 0; x < map.Width; x++)
        {
            for (int z = 0; z < map.Height; z++)
            {
                Vector3 position = new Vector3(x * config.DesiredSize.x, 0, z * config.DesiredSize.z);
                GameObject tileObject = SpawnTile(position, config.TilePrefab);

                Tile tile = tileObject.GetComponent<Tile>();
                if (tile != null)
                {
                    tile.Initialize(config.DesiredSize);
                    map.AddTile(tile, x, z);
                }
            }
        }
    }

    private GameObject SpawnTile(Vector3 position, GameObject prefab)
    {
        GameObject newTile = Instantiate(prefab, position, Quaternion.identity);
        newTile.transform.SetParent(mapContainer.transform);
        newTile.name = $"Tile_{position.x}_{position.z}";
        return newTile;
    }

    private Vector3 CalculateTileSize(GameObject prefab)
    {
        GameObject tempTile = Instantiate(prefab);
        Renderer renderer = tempTile.GetComponent<Renderer>();
        Vector3 size = renderer != null ? renderer.bounds.size : Vector3.one;
        Destroy(tempTile);

        Debug.Log($"Calculated tile size: {size}");
        return size;
    }
}
