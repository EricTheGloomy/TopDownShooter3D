using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector3 TileSize { get; private set; }
    public float TileHeight { get; private set; }

    public void Initialize(Vector3 desiredSize)
    {
        Renderer renderer = GetComponent<Renderer>();

        if (renderer == null)
        {
            Debug.LogError($"Tile {name} is missing a Renderer component.");
            TileSize = Vector3.zero;
            TileHeight = 0f;
            return;
        }

        TileSize = renderer.bounds.size;
        TileHeight = TileSize.y;

        Vector3 scaleFactor = new Vector3(
            desiredSize.x / TileSize.x,
            desiredSize.y / TileSize.y,
            desiredSize.z / TileSize.z
        );

        transform.localScale = Vector3.Scale(transform.localScale, scaleFactor);
        TileSize = renderer.bounds.size; // Update size after scaling.
        TileHeight = TileSize.y;
    }

    public Vector3 GetRandomPosition(float radius)
    {
        Vector3 tileCenter = transform.position;

        float safeMinX = tileCenter.x - (TileSize.x / 2) + radius;
        float safeMaxX = tileCenter.x + (TileSize.x / 2) - radius;
        float safeMinZ = tileCenter.z - (TileSize.z / 2) + radius;
        float safeMaxZ = tileCenter.z + (TileSize.z / 2) - radius;

        float randomX = Random.Range(safeMinX, safeMaxX);
        float randomZ = Random.Range(safeMinZ, safeMaxZ);

        return new Vector3(randomX, tileCenter.y + TileHeight / 2, randomZ);
    }
}
