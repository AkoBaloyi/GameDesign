using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class NailProjectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    public float damage = 10f;
    public float lifetime = 5f;
    public float gravity = 1f; // Gravity multiplier
    public bool useGravity = true;
    
    [Header("Effects")]
    public GameObject impactEffectPrefab;
    public AudioClip[] impactSounds;
    
    [Header("Surface Materials")]
    public PhysicMaterial woodMaterial;
    public PhysicMaterial metalMaterial;
    public PhysicMaterial concreteMaterial;
    
    private Rigidbody rb;
    private Collider col;
    private bool hasHit = false;
    private AudioSource audioSource;

    void Awake()
    {
        // Get required components
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        audioSource = GetComponent<AudioSource>();

        // Configure Rigidbody for projectile behavior
        SetupRigidbody();

        // Configure Collider
        SetupCollider();
        void Start()
        {
            // Destroy after lifetime
            Destroy(gameObject, lifetime);
        }
    }
    
    // Method to fire the nail with initial velocity
    public void FireNail(Vector3 direction, float speed)
    {
        if (rb != null)
        {
            rb.velocity = direction * speed;
        }
    }
    
    void SetupRigidbody()
    {
        if (rb == null) return;
        
        // Projectile physics settings
        rb.mass = 0.01f; // Very light
        rb.drag = 0.1f; // Slight air resistance
        rb.angularDrag = 0.5f;
        rb.useGravity = useGravity;
        
        // Adjust gravity if needed
        if (useGravity && gravity != 1f)
        {
            rb.gravityScale = gravity; // If using newer Unity version
            // For older versions, you'd apply gravity manually in FixedUpdate
        }
        
        // Prevent rotation on impact (nails should stick straight)
        rb.freezeRotation = true;
    }
    
    void SetupCollider()
    {
        if (col == null)
        {
            // Add collider if none exists
            col = gameObject.AddComponent<CapsuleCollider>();
        }
        
        // Configure for nail shape
        if (col is CapsuleCollider capsule)
        {
            capsule.radius = 0.005f; // Very thin
            capsule.height = 0.08f; // Nail length
            capsule.direction = 2; // Z-axis (forward)
        }
        
        // Ensure it's not a trigger initially
        col.isTrigger = false;
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (hasHit) return;
        
        hasHit = true;
        
        // Get impact point and normal
        ContactPoint contact = collision.contacts[0];
        Vector3 impactPoint = contact.point;
        Vector3 impactNormal = contact.normal;
        
        // Stick the nail to the surface
        StickToSurface(collision.transform, impactPoint, impactNormal);
        
        // Create impact effects
        CreateImpactEffects(impactPoint, impactNormal, collision.collider);
        
        // Play impact sound based on material
        PlayImpactSound(collision.collider);
        
        // Apply damage if target can take damage
        IDamageable damageable = collision.collider.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage);
        }
    }
    
    void StickToSurface(Transform surface, Vector3 point, Vector3 normal)
    {
        // Stop the nail
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }
        
        // Parent to the surface so it moves with it
        transform.SetParent(surface);
        
        // Position the nail at the impact point
        transform.position = point;
        
        // Align the nail with the surface normal (pointing into the surface)
        transform.rotation = Quaternion.LookRotation(-normal);
        
        // Push the nail slightly into the surface
        transform.position += normal * -0.02f;
    }
    
    void CreateImpactEffects(Vector3 point, Vector3 normal, Collider hitCollider)
    {
        // Create sparks or dust based on material
        GameObject effectPrefab = impactEffectPrefab;
        
        // Try to determine material type
        string materialType = DetermineMaterialType(hitCollider);
        
        if (effectPrefab != null)
        {
            GameObject effect = Instantiate(effectPrefab, point, Quaternion.LookRotation(normal));
            
            // Customize effect based on material
            ParticleSystem particles = effect.GetComponent<ParticleSystem>();
            if (particles != null)
            {
                var main = particles.main;
                
                switch (materialType.ToLower())
                {
                    case "metal":
                        main.startColor = Color.yellow; // Sparks
                        break;
                    case "wood":
                        main.startColor = new Color(0.6f, 0.4f, 0.2f); // Wood dust
                        break;
                    case "concrete":
                        main.startColor = Color.gray; // Concrete dust
                        break;
                    default:
                        main.startColor = Color.white;
                        break;
                }
            }
            
            // Destroy effect after a short time
            Destroy(effect, 2f);
        }
    }
    
    void PlayImpactSound(Collider hitCollider)
    {
        if (impactSounds == null || impactSounds.Length == 0) return;
        
        // Determine material and play appropriate sound
        string materialType = DetermineMaterialType(hitCollider);
        
        AudioClip soundToPlay = impactSounds[0]; // Default
        
        // You could have different sound arrays for different materials
        // For now, just use a random sound from the array
        soundToPlay = impactSounds[Random.Range(0, impactSounds.Length)];
        
        // Play the sound
        if (audioSource != null)
        {
            audioSource.PlayOneShot(soundToPlay);
        }
        else
        {
            AudioSource.PlayClipAtPoint(soundToPlay, transform.position);
        }
    }
    
    string DetermineMaterialType(Collider hitCollider)
    {
        // Check physics material
        if (hitCollider.material != null)
        {
            if (hitCollider.material == metalMaterial) return "metal";
            if (hitCollider.material == woodMaterial) return "wood";
            if (hitCollider.material == concreteMaterial) return "concrete";
        }
        
        // Check by tag
        switch (hitCollider.tag)
        {
            case "Metal":
                return "metal";
            case "Wood":
                return "wood";
            case "Concrete":
            case "Ground":
                return "concrete";
            default:
                return "generic";
        }
    }
    
    // Method to remove the nail (for cleanup or special mechanics)
    public void RemoveNail()
    {
        // Add a small pop effect
        if (impactEffectPrefab != null)
        {
            GameObject effect = Instantiate(impactEffectPrefab, transform.position, transform.rotation);
            Destroy(effect, 1f);
        }
        
        Destroy(gameObject);
    }
}

// Interface for objects that can take damage
public interface IDamageable
{
    void TakeDamage(float damage);
}