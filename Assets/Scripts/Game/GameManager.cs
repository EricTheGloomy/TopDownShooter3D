using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private MapManager mapManager;
    [SerializeField] private ObstacleSpawner obstacleSpawner;
    [SerializeField] private PlayerSpawner playerSpawner;
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private ObstacleManager obstacleManager; // Serialized for explicit assignment

    private void Start()
    {
        if (!ValidateDependencies())
        {
            Debug.LogError("GameManager initialization failed due to missing dependencies.");
            return;
        }

        InitializeGame();
    }

    private bool ValidateDependencies()
    {
        if (mapManager == null) Debug.LogError("MapManager is missing.");
        if (obstacleSpawner == null) Debug.LogError("ObstacleSpawner is missing.");
        if (playerSpawner == null) Debug.LogError("PlayerSpawner is missing.");
        if (playerManager == null) Debug.LogError("PlayerManager is missing.");
        if (cameraManager == null) Debug.LogError("CameraManager is missing.");
        if (enemySpawner == null) Debug.LogError("EnemySpawner is missing.");
        if (enemyManager == null) Debug.LogError("EnemyManager is missing.");
        if (obstacleManager == null) Debug.LogError("ObstacleManager is missing.");

        // Return false if any dependency is null
        return mapManager != null && obstacleSpawner != null &&
               playerSpawner != null && playerManager != null &&
               cameraManager != null && enemySpawner != null &&
               enemyManager != null && obstacleManager != null;
    }

    private void InitializeGame()
    {
        Debug.Log("Initializing Game...");

        mapManager.Initialize();
        obstacleSpawner.Initialize(mapManager.GetMap(), obstacleManager);
        playerSpawner.Initialize(mapManager.GetMap());

        if (playerManager.Player != null)
        {
            cameraManager.SetPlayer(playerManager.Player.transform);
            enemyManager.SetPlayerTransform(playerManager.Player.transform);
        }
        else
        {
            Debug.LogError("PlayerManager.Player is null during GameManager initialization.");
        }

        enemySpawner.Initialize(mapManager.GetMap(), playerManager.Player.transform);

        Debug.Log("Game Initialized!");
    }
}
