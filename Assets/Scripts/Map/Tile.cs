// Scripts/Map/Tile.cs
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector3 TileSize { get; private set; }
    public float TileHeight { get; private set; }

    public void Initialize(Vector3 desiredSize)
    {
        var renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            TileSize = renderer.bounds.size;
            TileHeight = TileSize.y;

            // Scale the tile to match the desired size
            var scaleFactor = new Vector3(
                desiredSize.x / TileSize.x,
                desiredSize.y / TileSize.y,
                desiredSize.z / TileSize.z
            );
            transform.localScale = Vector3.Scale(transform.localScale, scaleFactor);

            // Recalculate size after scaling
            TileSize = renderer.bounds.size;
            TileHeight = TileSize.y;
        }
        else
        {
            Debug.LogError($"Tile {name} is missing a Renderer component.");
            TileSize = Vector3.zero;
            TileHeight = 0f;
        }
    }

    public Vector3 GetRandomPosition(float radius)
    {
        var tileCenter = transform.position;
        float safeMinX = tileCenter.x - (TileSize.x / 2) + radius;
        float safeMaxX = tileCenter.x + (TileSize.x / 2) - radius;
        float safeMinZ = tileCenter.z - (TileSize.z / 2) + radius;
        float safeMaxZ = tileCenter.z + (TileSize.z / 2) - radius;

        float randomX = Random.Range(safeMinX, safeMaxX);
        float randomZ = Random.Range(safeMinZ, safeMaxZ);

        return new Vector3(randomX, tileCenter.y + TileHeight / 2, randomZ);
    }
}
