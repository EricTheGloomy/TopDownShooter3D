// Scripts/Game/GameManager.cs
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private MapManager mapManager;
    private ObstacleSpawner obstacleSpawner;
    private PlayerSpawner playerSpawner;
    private PlayerManager playerManager;
    private CameraManager cameraManager;
    private EnemySpawner enemySpawner;
    private EnemyManager enemyManager;

    void Start()
    {
        mapManager = FindObjectOfType<MapManager>();
        obstacleSpawner = FindObjectOfType<ObstacleSpawner>();
        playerSpawner = FindObjectOfType<PlayerSpawner>();
        playerManager = FindObjectOfType<PlayerManager>();
        cameraManager = FindObjectOfType<CameraManager>();
        enemySpawner = FindObjectOfType<EnemySpawner>();
        enemyManager = FindObjectOfType<EnemyManager>();

        if (mapManager == null || obstacleSpawner == null || playerSpawner == null || 
            playerManager == null || cameraManager == null || enemySpawner == null || enemyManager == null)
        {
            Debug.LogError("GameManager dependencies are missing.");
            return;
        }

        InitializeGame();
    }

    private void InitializeGame()
    {
        Debug.Log("Initializing Game...");

        mapManager.Initialize();
        obstacleSpawner.Initialize(mapManager.map, FindObjectOfType<ObstacleManager>());
        playerSpawner.Initialize(mapManager.map);

        if (playerManager.Player != null)
        {
            cameraManager.SetPlayer(playerManager.Player.transform); // Ensure SetPlayer is called after player initialization.
        }
        else
        {
            Debug.LogError("PlayerManager.Player is null during GameManager initialization.");
        }

        enemySpawner.Initialize(mapManager.map, FindObjectOfType<ObstacleManager>());

        Debug.Log("Game Initialized!");
    }

}
