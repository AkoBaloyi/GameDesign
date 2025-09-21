using UnityEngine;

public class NailProjectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    public float damage = 10f;
    public float lifetime = 5f;
    public float stickForce = 100f;
    
    [Header("Effects")]
    public GameObject impactEffectPrefab;
    public AudioClip[] impactSounds;
    
    [Header("Surface Materials")]
    public PhysicsMaterial woodMaterial;
    public PhysicsMaterial metalMaterial;
    public PhysicsMaterial concreteMaterial;
    
    private Rigidbody rb;
    private bool hasHit = false;
    private AudioSource audioSource;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        
        // Destroy after lifetime
        Destroy(gameObject, lifetime);
        
        // Ensure the nail has a collider
        if (GetComponent<Collider>() == null)
        {
            CapsuleCollider collider = gameObject.AddComponent<CapsuleCollider>();
            collider.radius = 0.01f;
            collider.height = 0.1f;
        }
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
            rb.linearVelocity = Vector3.zero;
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