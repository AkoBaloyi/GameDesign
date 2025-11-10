using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// FAST Enemy AI - Directly chases player with REAL speed
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class SimpleEnemyAI : MonoBehaviour
{
    [Header("Detection & Speed")]
    public float detectionRange = 30f;
    public float killDistance = 2f;
    public float chaseSpeed = 15f; // This is the REAL speed now
    public float gravity = 20f; // Keep enemies on ground
    
    [Header("References")]
    public Transform player;
    
    private NavMeshAgent agent;
    private PlayerHealth playerHealth;
    private CharacterController controller;
    private bool isChasing = false;
    private float verticalVelocity = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        controller = GetComponent<CharacterController>();
        
        if (agent == null)
        {
            Debug.LogError($"[SimpleEnemyAI] {gameObject.name} missing NavMeshAgent!");
            enabled = false;
            return;
        }
        
        // DISABLE NavMesh movement - we'll move manually for REAL speed
        agent.updatePosition = false;
        agent.updateRotation = false;
        
        // Find player
        FindPlayer();
        
        Debug.Log($"[SimpleEnemyAI] {gameObject.name} ready to hunt at REAL speed {chaseSpeed}!");
    }

    void FindPlayer()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
                playerHealth = playerObj.GetComponent<PlayerHealth>();
                Debug.Log($"[SimpleEnemyAI] Found player: {player.name}");
            }
            else
            {
                Debug.LogError("[SimpleEnemyAI] No GameObject with tag 'Player' found!");
            }
        }
    }

    void Update()
    {
        // Find player if not set
        if (player == null)
        {
            FindPlayer();
            if (player == null) return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // KILL CHECK
        if (distanceToPlayer <= killDistance)
        {
            if (playerHealth != null && !playerHealth.isDead)
            {
                Debug.LogWarning($"[SimpleEnemyAI] KILLED PLAYER at distance {distanceToPlayer:F2}!");
                playerHealth.Die();
                return;
            }
        }

        // CHASE PLAYER if in range
        if (distanceToPlayer <= detectionRange)
        {
            if (!isChasing)
            {
                isChasing = true;
                Debug.Log($"[SimpleEnemyAI] {gameObject.name} CHASING at speed {chaseSpeed}!");
            }

            // MOVE DIRECTLY TOWARD PLAYER - NO NAVMESH SLOWDOWN!
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            directionToPlayer.y = 0; // Keep movement horizontal only
            
            Vector3 moveVector = directionToPlayer * chaseSpeed * Time.deltaTime;
            
            // Apply gravity
            if (controller != null && controller.isGrounded)
            {
                verticalVelocity = -2f; // Small downward force to stay grounded
            }
            else
            {
                verticalVelocity -= gravity * Time.deltaTime;
            }
            
            moveVector.y = verticalVelocity * Time.deltaTime;
            
            // Move with CharacterController if available, otherwise direct transform
            if (controller != null)
            {
                controller.Move(moveVector);
            }
            else
            {
                transform.position += moveVector;
            }
            
            // Rotate to face player
            if (directionToPlayer != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
            }
        }
        else
        {
            // Lost player
            if (isChasing)
            {
                isChasing = false;
                Debug.Log($"[SimpleEnemyAI] {gameObject.name} lost player");
            }
        }
    }

    // Backup collision detection
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (playerHealth != null && !playerHealth.isDead)
            {
                Debug.LogWarning("[SimpleEnemyAI] COLLISION KILL!");
                playerHealth.Die();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerHealth != null && !playerHealth.isDead)
            {
                Debug.LogWarning("[SimpleEnemyAI] TRIGGER KILL!");
                playerHealth.Die();
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw detection range (red)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        
        // Draw kill range (yellow)
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, killDistance);
    }
}
