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
        agent.speed = patrolSpeed;
        
        // Start patrolling if we have waypoints
        if (patrolWaypoints != null && patrolWaypoints.Length > 0)
        {
            GoToNextWaypoint();
        }
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

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
