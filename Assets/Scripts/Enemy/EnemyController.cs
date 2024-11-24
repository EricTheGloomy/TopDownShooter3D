using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private EnemyControllerSettings settings;

    private float updateTimer;

    void Start()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError($"NavMeshAgent missing on {name}.");
            return;
        }

        if (settings == null)
        {
            Debug.LogWarning($"EnemyControllerSettings not assigned for {name}. Default values will be used.");
        }

        InitializeAgent();

        if (playerTransform == null)
        {
            Debug.LogWarning("Player Transform not assigned. Enemy cannot track the player.");
        }
    }

    void Update()
    {
        if (playerTransform == null || agent == null || settings == null) return;

        updateTimer += Time.deltaTime;

        if (updateTimer >= settings.TargetUpdateInterval)
        {
            updateTimer = 0f;
            UpdateAgentDestination();
        }
    }

    public void SetPlayerTransform(Transform player)
    {
        playerTransform = player;
    }

    public void InitializeAgent()
    {
        agent.stoppingDistance = settings.StoppingDistance;
        agent.autoRepath = true;
        agent.acceleration = settings.Acceleration;

        var enemy = GetComponent<Enemy>();
        if (enemy != null)
        {
            agent.speed = enemy.GetCurrentSpeed();
        }
        else
        {
            Debug.LogWarning($"Enemy component not found on {name}. Using default speed.");
            agent.speed = 3f; // Default fallback.
        }
    }

    private void UpdateAgentDestination()
    {
        if (agent != null && playerTransform != null)
        {
            agent.SetDestination(playerTransform.position);
        }
    }
}
