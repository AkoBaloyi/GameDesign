using UnityEngine;

/// <summary>
/// Enemy health system - Takes damage and dies
/// </summary>
public class EnemyHealth : MonoBehaviour, IDamageable
{
    [Header("Health")]
    public float maxHealth = 100f;
    public float currentHealth;
    
    [Header("Visual Feedback")]
    public GameObject deathEffect;
    public Material damageMaterial;
    public float damageFeedbackDuration = 0.1f;
    
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip hitSound;
    public AudioClip deathSound;
    
    [Header("Drops")]
    public GameObject[] itemsToDrop;
    public float dropForce = 5f;
    
    private Renderer enemyRenderer;
    private Material originalMaterial;
    private bool isDead = false;

    void Awake()
    {
        currentHealth = maxHealth;
        enemyRenderer = GetComponentInChildren<Renderer>();
        
        if (enemyRenderer != null)
        {
            originalMaterial = enemyRenderer.material;
        }
        
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        
        Debug.Log($"[Enemy {gameObject.name}] Took {damage} damage. Health: {currentHealth}/{maxHealth}");

        // Play hit sound
        if (audioSource != null && hitSound != null)
        {
            audioSource.PlayOneShot(hitSound);
        }

        // Visual feedback
        if (enemyRenderer != null && damageMaterial != null)
        {
            StartCoroutine(DamageFlash());
        }

        // Check if dead
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    System.Collections.IEnumerator DamageFlash()
    {
        if (enemyRenderer != null && damageMaterial != null)
        {
            enemyRenderer.material = damageMaterial;
            yield return new WaitForSeconds(damageFeedbackDuration);
            enemyRenderer.material = originalMaterial;
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log($"[Enemy {gameObject.name}] Died!");

        // Play death sound
        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        // Spawn death effect
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }

        // Drop items
        DropItems();

        // Disable AI and collisions
        var ai = GetComponent<SimpleEnemyAI>();
        if (ai != null) ai.enabled = false;

        var agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null) agent.enabled = false;

        var collider = GetComponent<Collider>();
        if (collider != null) collider.enabled = false;

        // Destroy after delay (let death sound play)
        Destroy(gameObject, 2f);
    }

    void DropItems()
    {
        if (itemsToDrop == null || itemsToDrop.Length == 0) return;

        foreach (var item in itemsToDrop)
        {
            if (item != null)
            {
                GameObject dropped = Instantiate(item, transform.position + Vector3.up, Quaternion.identity);
                
                // Add some force for dramatic effect
                Rigidbody rb = dropped.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Vector3 randomDirection = new Vector3(
                        Random.Range(-1f, 1f),
                        Random.Range(0.5f, 1f),
                        Random.Range(-1f, 1f)
                    ).normalized;
                    
                    rb.AddForce(randomDirection * dropForce, ForceMode.Impulse);
                }
            }
        }
    }

    // Public method to check if enemy is alive
    public bool IsAlive()
    {
        return !isDead;
    }

    // Public method to get health percentage
    public float GetHealthPercentage()
    {
        return currentHealth / maxHealth;
    }
}
