using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector3 TileSize { get; private set; }
    public float TileHeight { get; private set; }

    public void Initialize(Vector3 desiredSize)
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            TileSize = renderer.bounds.size;
            TileHeight = TileSize.y;

            // Calculate and apply scale factor
            Vector3 scaleFactor = new Vector3(
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
}
