using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform playerTransform;
    public float targetUpdateInterval = 0.2f; // Time between updates
    private float updateTimer = 0f;          // Tracks time since last update

    public int obstacleAvoidancePriority = 50; // Default priority

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerTransform = FindObjectOfType<Player>()?.transform;

        if (agent != null)
        {
            agent.stoppingDistance = 0.5f;
            agent.autoRepath = true;
            agent.acceleration = 8f;
            agent.avoidancePriority = obstacleAvoidancePriority;
            agent.speed = GetComponent<Enemy>()?.GetCurrentSpeed() ?? 3f;
        }

        if (playerTransform == null)
        {
            Debug.LogError("Player not found. EnemyController will not function properly.");
        }
    }

    void Update()
    {
        updateTimer += Time.deltaTime;

        // Only update the target if the interval has elapsed
        if (updateTimer >= targetUpdateInterval)
        {
            updateTimer = 0f;

            if (playerTransform != null && agent != null)
            {
                agent.SetDestination(playerTransform.position);
            }
        }
    }

    public void SetObstaclePriority(int priority)
    {
        if (agent != null)
        {
            agent.avoidancePriority = priority;
        }
    }
}
