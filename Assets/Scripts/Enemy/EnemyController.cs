using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;       // Reference to NavMeshAgent
    [SerializeField] private Transform playerTransform; // Assigned by GameManager or other systems
    [SerializeField] private EnemyControllerSettings settings; // ScriptableObject for settings

    private float updateTimer = 0f; // Timer for throttling updates

    void Start()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogError($"NavMeshAgent component is missing on {name}. EnemyController will not function.");
            return;
        }

        if (settings == null)
        {
            Debug.LogWarning($"EnemyControllerSettings not assigned on {name}. Using fallback serialized values.");
        }

        InitializeAgent();

        if (playerTransform == null)
        {
            Debug.LogError("Player Transform is not assigned to EnemyController.");
        }
    }

    void Update()
    {
        if (playerTransform == null || agent == null || settings == null) return;

        updateTimer += Time.deltaTime;

        if (updateTimer >= settings.targetUpdateInterval)
        {
            updateTimer = 0f;
            UpdateAgentDestination();
        }
    }

    public void SetPlayerTransform(Transform player)
    {
        playerTransform = player;
    }

    public void SetObstaclePriority(int priority)
    {
        if (agent != null)
        {
            agent.avoidancePriority = priority;
        }
    }

    public void InitializeAgent()
    {
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
            if (agent == null)
            {
                Debug.LogError($"NavMeshAgent component is missing on {name}. EnemyController will not function.");
                return;
            }
        }
        
        agent.stoppingDistance = settings.stoppingDistance;
        agent.autoRepath = true;
        agent.acceleration = settings.acceleration;

        var enemy = GetComponent<Enemy>();
        if (enemy != null)
        {
            agent.speed = enemy.GetCurrentSpeed();
        }
        else
        {
            Debug.LogWarning($"Enemy component not found on {name}. Using default speed for NavMeshAgent.");
            agent.speed = 3f;
        }
    }

    private void UpdateAgentDestination()
    {
        if (playerTransform != null && agent != null)
        {
            agent.SetDestination(playerTransform.position);
        }
    }
}
