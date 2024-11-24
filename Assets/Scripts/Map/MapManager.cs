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
        if (mapSpawner == null || navMeshSurface == null || map == null || mapConfig == null)
        {
            throw new MissingReferenceException("MapManager dependencies are not assigned.");
        }

        map.Initialize(mapConfig.gridWidth, mapConfig.gridHeight);
        mapSpawner.PopulateMap(map, mapConfig.tileConfig);
        BakeNavMesh();
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
