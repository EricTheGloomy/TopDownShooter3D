// Scripts/Map/MapManager.cs
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class MapManager : MonoBehaviour
{
    [SerializeField] public MapSpawner mapSpawner; // Reference to the MapSpawner
    [SerializeField] private NavMeshSurface navMeshSurface; // Reference to the NavMeshSurface
    [SerializeField] public Map map; // Reference to the Map script

    public void Initialize()
    {
        Debug.Log("Initializing MapManager...");

        if (mapSpawner == null || navMeshSurface == null || map == null)
        {
            Debug.LogError("MapManager dependencies are not assigned.");
            return;
        }

        // Initialize the Map
        map.Initialize(mapSpawner.gridWidth, mapSpawner.gridHeight);
        mapSpawner.PopulateMap(map);

        // Bake the NavMesh after tiles are placed
        BakeNavMesh();

        Debug.Log("MapManager initialized.");
    }

    private void BakeNavMesh()
    {
        Debug.Log("Baking NavMesh...");
        navMeshSurface.BuildNavMesh();
        Debug.Log("NavMesh baked successfully.");
    }
}
