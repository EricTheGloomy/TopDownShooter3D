using Unity.AI.Navigation;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] private MapSpawner mapSpawner;
    [SerializeField] private NavMeshSurface navMeshSurface;
    [SerializeField] private Map map;
    [SerializeField] private MapConfig mapConfig;

    public Map GetMap() => map;

    public void Initialize()
    {
        if (!ValidateDependencies()) return;

        map.Initialize(mapConfig.GridWidth, mapConfig.GridHeight);
        mapSpawner.PopulateMap(map, mapConfig.TileConfig);
        BakeNavMesh();
    }

    private bool ValidateDependencies()
    {
        bool isValid = true;

        if (mapSpawner == null)
        {
            Debug.LogError("MapSpawner is missing.");
            isValid = false;
        }
        if (navMeshSurface == null)
        {
            Debug.LogError("NavMeshSurface is missing.");
            isValid = false;
        }
        if (map == null)
        {
            Debug.LogError("Map is missing.");
            isValid = false;
        }
        if (mapConfig == null)
        {
            Debug.LogError("MapConfig is missing.");
            isValid = false;
        }

        return isValid;
    }

    private void BakeNavMesh()
    {
        if (map.GetAllTiles() == null)
        {
            Debug.LogError("Cannot bake NavMesh: No tiles in the map.");
            return;
        }

        Debug.Log("Baking NavMesh...");
        navMeshSurface.BuildNavMesh();
        Debug.Log("NavMesh baked successfully.");
    }
}
