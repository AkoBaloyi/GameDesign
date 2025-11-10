using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// SMART Enemy AI - Actually tries to catch and kill player
/// Avoids other enemies, aggressive pursuit, kills on contact
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class SmartEnemyAI : MonoBehaviour
{
    [Header("Behavior")]
    public float detectionRange = 30f;
    public float killDistance = 1.5f;
    public float chaseSpeed = 8f;
    public float aggressiveSpeed = 12f; // Speed when very close
    
    [Header("Avoidance")]
    public float enemyAvoidanceRadius = 2f;
    public LayerMask enemyLayer;
    
    private NavMeshAgent agent;
    private Transform player;
    private PlayerHealth playerHealth;
    private bool isChasing = false;
    private float updatePathTimer = 0f;
    private float updatePathInterval = 0.2f; // Update path 5 times per second

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        
        // Find player
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerHealth = playerObj.GetComponent<PlayerHealth>();
        }
    }

    void Start()
    {
        if (agent == null)
        {
            Debug.LogError($"[SmartEnemyAI] {gameObject.name} has no NavMeshAgent!");
            enabled = false;
            return;
        }
        
        // Configure NavMeshAgent for smart behavior
        agent.speed = chaseSpeed;
        agent.acceleration = 12f;
        agent.angularSpeed = 200f;
        agent.stoppingDistance = 0.5f; // Get VERY close
        agent.autoBraking = false; // Don't slow down
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        agent.avoidancePriority = Random.Range(30, 70); // Random priority so they don't all behave the same
        
        // Prevent sinking
        agent.baseOffset = 0f;
        agent.height = 1.8f;
        agent.radius = 0.4f;
        
        Debug.Log($"[SmartEnemyAI] {gameObject.name} initialized - HUNTING MODE");
    }

    void Update()
    {
        // Find player if lost
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
                playerHealth = playerObj.GetComponent<PlayerHealth>();
            }
            else
            {
                return; // No player, can't do anything
            }
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // KILL CHECK - if close enough, kill player immediately
        if (distanceToPlayer <= killDistance)
        {
            KillPlayer();
            return;
        }

        // DETECTION - start chasing if player in range
        if (distanceToPlayer <= detectionRange)
        {
            if (!isChasing)
            {
                isChasing = true;
                Debug.Log($"[SmartEnemyAI] {gameObject.name} DETECTED PLAYER - ENGAGING!");
            }
            
            ChasePlayer(distanceToPlayer);
        }
        else
        {
            // Player out of range
            if (isChasing)
            {
                isChasing = false;
                agent.ResetPath();
            }
        }
    }

    void ChasePlayer(float distance)
    {
        // Speed up when close
        if (distance < 5f)
        {
            agent.speed = aggressiveSpeed; // FAST when close!
        }
        else
        {
            agent.speed = chaseSpeed;
        }

        // Update path periodically (not every frame for performance)
        updatePathTimer += Time.deltaTime;
        if (updatePathTimer >= updatePathInterval)
        {
            updatePathTimer = 0f;
            
            // Calculate path to player
            Vector3 targetPosition = player.position;
            
            // Avoid other enemies
            Vector3 avoidanceVector = CalculateEnemyAvoidance();
            if (avoidanceVector != Vector3.zero)
            {
                // Adjust target to avoid other enemies
                targetPosition += avoidanceVector;
            }
            
            agent.SetDestination(targetPosition);
        }
    }

    Vector3 CalculateEnemyAvoidance()
    {
        // Find nearby enemies
        Collider[] nearbyEnemies = Physics.OverlapSphere(transform.position, enemyAvoidanceRadius, enemyLayer);
        
        if (nearbyEnemies.Length <= 1) // Only self
            return Vector3.zero;
        
        Vector3 avoidanceVector = Vector3.zero;
        int count = 0;
        
        foreach (Collider enemy in nearbyEnemies)
        {
            if (enemy.gameObject == gameObject) continue; // Skip self
            
            // Calculate direction away from other enemy
            Vector3 directionAway = transform.position - enemy.transform.position;
            float distance = directionAway.magnitude;
            
            if (distance < enemyAvoidanceRadius && distance > 0.1f)
            {
                // Closer enemies have more influence
                avoidanceVector += directionAway.normalized / distance;
                count++;
            }
        }
        
        if (count > 0)
        {
            avoidanceVector = avoidanceVector.normalized * 2f; // Avoidance strength
        }
        
        return avoidanceVector;
    }

    void KillPlayer()
    {
        if (playerHealth != null && !playerHealth.isDead)
        {
            Debug.LogWarning($"[SmartEnemyAI] {gameObject.name} KILLED PLAYER!");
            playerHealth.Die();
            
            // Stop moving after kill
            agent.ResetPath();
            isChasing = false;
        }
    }

    // Also catch with collision (backup method)
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            KillPlayer();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            KillPlayer();
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw detection range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        
        // Draw kill range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, killDistance);
        
        // Draw avoidance range
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, enemyAvoidanceRadius);
    }
}
