using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Simple enemy AI - Patrols waypoints and chases player when in range
/// No shooting, just follows player
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class SimpleEnemyAI : MonoBehaviour
{
    [Header("Behavior")]
    public float detectionRange = 15f;
    public float chaseRange = 25f;
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;
    
    [Header("Patrol")]
    public Transform[] patrolWaypoints;
    public float waypointReachDistance = 2f;
    public float waitTimeAtWaypoint = 2f;
    
    [Header("References")]
    public Transform player;
    
    private NavMeshAgent agent;
    private int currentWaypointIndex = 0;
    private float waitTimer = 0f;
    private bool isWaiting = false;
    
    public enum State
    {
        Patrolling,
        Chasing,
        Waiting
    }
    
    public State currentState = State.Patrolling;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        
        // Find player if not assigned
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }
    }

    void Start()
    {
        // Check if agent exists
        if (agent == null)
        {
            Debug.LogError($"[SimpleEnemyAI] {gameObject.name} has no NavMeshAgent component!");
            return;
        }
        
        agent.speed = patrolSpeed;
        
        // FIX: Prevent sinking into floor
        agent.baseOffset = 0f; // Keep on surface
        agent.height = 1.8f; // Match capsule collider
        agent.radius = 0.3f; // Match capsule collider
        
        // Check if on NavMesh
        if (!agent.isOnNavMesh)
        {
            Debug.LogError($"[SimpleEnemyAI] {gameObject.name} is NOT on NavMesh! Position: {transform.position}");
            Debug.LogError("FIX: Bake NavMesh (Window → AI → Navigation → Bake) or move enemy to NavMesh surface");
            return;
        }
        
        Debug.Log($"[SimpleEnemyAI] {gameObject.name} initialized. On NavMesh: {agent.isOnNavMesh}, Speed: {agent.speed}");
        
        // Start patrolling if we have waypoints
        if (patrolWaypoints != null && patrolWaypoints.Length > 0)
        {
            Debug.Log($"[SimpleEnemyAI] {gameObject.name} has {patrolWaypoints.Length} waypoints. Starting patrol.");
            GoToNextWaypoint();
        }
        else
        {
            Debug.LogWarning($"[SimpleEnemyAI] {gameObject.name} has NO waypoints. Will only chase player when detected.");
        }
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // CHECK FOR COLLISION MANUALLY (since NavMesh agents don't trigger collisions well)
        if (distanceToPlayer < 1.5f) // Close enough = caught!
        {
            Debug.Log("[SimpleEnemyAI] CAUGHT PLAYER! Distance: " + distanceToPlayer);
            // Trigger death on player
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                // Player will handle death
                Debug.Log("[SimpleEnemyAI] Calling player death!");
            }
        }

        // State machine
        switch (currentState)
        {
            case State.Patrolling:
                HandlePatrol(distanceToPlayer);
                break;
                
            case State.Chasing:
                HandleChase(distanceToPlayer);
                break;
                
            case State.Waiting:
                HandleWaiting(distanceToPlayer);
                break;
        }
    }

    void HandlePatrol(float distanceToPlayer)
    {
        // Check if player is in detection range
        if (distanceToPlayer <= detectionRange)
        {
            StartChasing();
            return;
        }

        // Continue patrolling
        if (patrolWaypoints == null || patrolWaypoints.Length == 0) return;

        // Check if reached waypoint
        if (!agent.pathPending && agent.remainingDistance <= waypointReachDistance)
        {
            StartWaiting();
        }
    }

    void HandleChase(float distanceToPlayer)
    {
        // Lost player - go back to patrol
        if (distanceToPlayer > chaseRange)
        {
            StartPatrolling();
            return;
        }

        // Chase player
        agent.SetDestination(player.position);
    }

    void HandleWaiting(float distanceToPlayer)
    {
        // Check if player is in detection range while waiting
        if (distanceToPlayer <= detectionRange)
        {
            StartChasing();
            return;
        }

        // Wait at waypoint
        waitTimer += Time.deltaTime;
        if (waitTimer >= waitTimeAtWaypoint)
        {
            waitTimer = 0f;
            isWaiting = false;
            GoToNextWaypoint();
            currentState = State.Patrolling;
        }
    }

    void StartChasing()
    {
        currentState = State.Chasing;
        agent.speed = chaseSpeed;
        agent.SetDestination(player.position);
        Debug.Log($"[Enemy {gameObject.name}] Started chasing player!");
    }

    void StartPatrolling()
    {
        currentState = State.Patrolling;
        agent.speed = patrolSpeed;
        
        if (patrolWaypoints != null && patrolWaypoints.Length > 0)
        {
            GoToNextWaypoint();
        }
        
        Debug.Log($"[Enemy {gameObject.name}] Returned to patrol");
    }

    void StartWaiting()
    {
        currentState = State.Waiting;
        isWaiting = true;
        waitTimer = 0f;
    }

    void GoToNextWaypoint()
    {
        if (patrolWaypoints == null || patrolWaypoints.Length == 0) return;

        agent.SetDestination(patrolWaypoints[currentWaypointIndex].position);
        currentWaypointIndex = (currentWaypointIndex + 1) % patrolWaypoints.Length;
    }

    // Catch player on collision - instant death!
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("[SimpleEnemyAI] Caught player! Game Over!");
            // PlayerHealth script will handle the death
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("[SimpleEnemyAI] Caught player! Game Over!");
            // PlayerHealth script will handle the death
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw detection range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        
        // Draw chase range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
        
        // Draw patrol waypoints
        if (patrolWaypoints != null && patrolWaypoints.Length > 0)
        {
            Gizmos.color = Color.blue;
            for (int i = 0; i < patrolWaypoints.Length; i++)
            {
                if (patrolWaypoints[i] != null)
                {
                    Gizmos.DrawSphere(patrolWaypoints[i].position, 0.3f);
                    
                    // Draw line to next waypoint
                    int nextIndex = (i + 1) % patrolWaypoints.Length;
                    if (patrolWaypoints[nextIndex] != null)
                    {
                        Gizmos.DrawLine(patrolWaypoints[i].position, patrolWaypoints[nextIndex].position);
                    }
                }
            }
        }
    }
}
