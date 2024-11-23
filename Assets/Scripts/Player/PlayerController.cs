// Scripts/Player/PlayerController.cs
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 360f; // Degrees per second

    private Rigidbody rb;
    private NavMeshAgent agent;
    private NavMeshPath navPath;
    private Vector3 lastValidPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();

        // Disable NavMeshAgent auto-movement but keep it for validation
        agent.updatePosition = false;
        agent.updateRotation = false;

        navPath = new NavMeshPath();

        // Initialize player's starting position as valid
        lastValidPosition = transform.position;
    }

    void FixedUpdate()
    {
        // Get input
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        if (direction.magnitude > 0.1f)
        {
            // Calculate desired position
            Vector3 desiredPosition = rb.position + direction * moveSpeed * Time.fixedDeltaTime;

            // Validate position across NavMesh chunks
            if (NavMesh.CalculatePath(lastValidPosition, desiredPosition, NavMesh.AllAreas, navPath) && navPath.status == NavMeshPathStatus.PathComplete)
            {
                lastValidPosition = desiredPosition; // Update valid position
            }

            // Move Rigidbody to last valid position
            rb.MovePosition(lastValidPosition);

            // Rotate to face movement direction
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
        else
        {
            // Stop all movement immediately
            rb.velocity = Vector3.zero;
        }
    }
}
