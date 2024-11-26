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
    [SerializeField] private ObstacleManager obstacleManager;
    [SerializeField] private WaveManager waveManager;

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
        bool allValid = true;

        foreach (var field in GetType().GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance))
        {
            if (field.GetCustomAttributes(typeof(SerializeField), true).Length > 0)
            {
                if (field.GetValue(this) == null)
                {
                    Debug.LogError($"{field.Name} is missing in {GetType().Name}.");
                    allValid = false;
                }
            }
        }

        return allValid;
    }

    private void InitializeGame()
    {
        Debug.Log("Initializing Game...");

        mapManager.Initialize();
        obstacleSpawner.Initialize(mapManager.GetMap());
        playerSpawner.Initialize(mapManager.GetMap());

        var player = playerManager.Player;
        if (player != null)
        {
            cameraManager.SetPlayer(player.transform);
            enemyManager.SetPlayerTransform(player.transform);
        }
        else
        {
            Debug.LogError("PlayerManager.Player is null during GameManager initialization.");
            return;
        }

        enemySpawner.Initialize(mapManager.GetMap(), player.transform);

        // Initialize the WaveManager after all dependencies are set up
        if (waveManager != null)
        {
            Debug.Log("Initializing WaveManager...");
            waveManager.enabled = true; // Ensure it's enabled before starting waves
        }
        else
        {
            Debug.LogError("WaveManager is missing in GameManager.");
        }

        Debug.Log("Game Initialized!");
    }
}
