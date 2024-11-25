using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputSettings inputSettings; // New InputSettings

    private Rigidbody rb;
    private NavMeshAgent agent;
    private NavMeshPath navPath;
    private Vector3 lastValidPosition;

    private Player player;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        player = GetComponent<Player>();

        if (inputSettings == null)
        {
            Debug.LogError("InputSettings is not assigned!");
            enabled = false;
            return;
        }

        navPath = new NavMeshPath();

        InitializeNavMeshAgent();

        lastValidPosition = transform.position;
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector3 inputDirection = GetInputDirection();

        if (inputDirection.magnitude > 0.1f)
        {
            ProcessMovement(inputDirection);
        }
        else
        {
            rb.velocity = Vector3.zero; // Stop movement if no input
        }
    }

    private Vector3 GetInputDirection()
    {
        float horizontal = Input.GetAxisRaw(inputSettings.horizontalAxis);
        float vertical = Input.GetAxisRaw(inputSettings.verticalAxis);
        return new Vector3(horizontal, 0, vertical).normalized;
    }

    private void ProcessMovement(Vector3 direction)
    {
        Vector3 desiredPosition = rb.position + direction * player.GetMovementSpeed() * Time.fixedDeltaTime;

        if (NavMesh.CalculatePath(lastValidPosition, desiredPosition, NavMesh.AllAreas, navPath) &&
            navPath.status == NavMeshPathStatus.PathComplete)
        {
            lastValidPosition = desiredPosition;
        }

        rb.MovePosition(lastValidPosition);

        Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, player.config.rotationSpeed * Time.fixedDeltaTime);
    }

    private void InitializeNavMeshAgent()
    {
        agent.updatePosition = false;
        agent.updateRotation = false;
    }
}
