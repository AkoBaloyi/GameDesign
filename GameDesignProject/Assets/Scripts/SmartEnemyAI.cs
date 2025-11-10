using UnityEngine;
using UnityEngine.AI;




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

        agent.speed = chaseSpeed;
        agent.acceleration = 12f;
        agent.angularSpeed = 200f;
        agent.stoppingDistance = 0.5f; // Get VERY close
        agent.autoBraking = false; // Don't slow down
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        agent.avoidancePriority = Random.Range(30, 70); // Random priority so they don't all behave the same

        agent.baseOffset = 0f;
        agent.height = 1.8f;
        agent.radius = 0.4f;
        
        Debug.Log($"[SmartEnemyAI] {gameObject.name} initialized - HUNTING MODE");
    }

    void Update()
    {

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

        if (distanceToPlayer <= killDistance)
        {
            KillPlayer();
            return;
        }

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

            if (isChasing)
            {
                isChasing = false;
                agent.ResetPath();
            }
        }
    }

    void ChasePlayer(float distance)
    {

        if (distance < 5f)
        {
            agent.speed = aggressiveSpeed; // FAST when close!
        }
        else
        {
            agent.speed = chaseSpeed;
        }

        updatePathTimer += Time.deltaTime;
        if (updatePathTimer >= updatePathInterval)
        {
            updatePathTimer = 0f;

            Vector3 targetPosition = player.position;

            Vector3 avoidanceVector = CalculateEnemyAvoidance();
            if (avoidanceVector != Vector3.zero)
            {

                targetPosition += avoidanceVector;
            }
            
            agent.SetDestination(targetPosition);
        }
    }

    Vector3 CalculateEnemyAvoidance()
    {

        Collider[] nearbyEnemies = Physics.OverlapSphere(transform.position, enemyAvoidanceRadius, enemyLayer);
        
        if (nearbyEnemies.Length <= 1) // Only self
            return Vector3.zero;
        
        Vector3 avoidanceVector = Vector3.zero;
        int count = 0;
        
        foreach (Collider enemy in nearbyEnemies)
        {
            if (enemy.gameObject == gameObject) continue; // Skip self

            Vector3 directionAway = transform.position - enemy.transform.position;
            float distance = directionAway.magnitude;
            
            if (distance < enemyAvoidanceRadius && distance > 0.1f)
            {

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

            agent.ResetPath();
            isChasing = false;
        }
    }

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

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, killDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, enemyAvoidanceRadius);
    }
}
