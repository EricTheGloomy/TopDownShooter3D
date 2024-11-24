// Scripts/Player/PlayerController.cs
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
public class PlayerController : MonoBehaviour
{
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

        navPath = new NavMeshPath();

        InitializeNavMeshAgent();

        lastValidPosition = transform.position;
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        if (direction.magnitude > 0.1f)
        {
            Vector3 desiredPosition = rb.position + direction * player.GetCurrentSpeed() * Time.fixedDeltaTime;

            if (NavMesh.CalculatePath(lastValidPosition, desiredPosition, NavMesh.AllAreas, navPath) && navPath.status == NavMeshPathStatus.PathComplete)
            {
                lastValidPosition = desiredPosition;
            }

            rb.MovePosition(lastValidPosition);

            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, player.config.rotationSpeed * Time.fixedDeltaTime);
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }

    private void InitializeNavMeshAgent()
    {
        agent.updatePosition = false;
        agent.updateRotation = false;
    }
}
