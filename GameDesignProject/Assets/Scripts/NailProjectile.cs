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
    public PhysicsMaterial woodMaterial;
    public PhysicsMaterial metalMaterial;
    public PhysicsMaterial concreteMaterial;
    
    private Rigidbody rb;
    private Collider col;
    private bool hasHit = false;
    private AudioSource audioSource;

    void Awake()
    {

        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        audioSource = GetComponent<AudioSource>();

        SetupRigidbody();

        SetupCollider();
        void Start()
        {

            Destroy(gameObject, lifetime);
        }
    }

    public void FireNail(Vector3 direction, float speed)
    {
        if (rb != null)
        {
            rb.linearVelocity = direction * speed;
        }
    }
    
    void SetupRigidbody()
    {
        if (rb == null) return;

        rb.mass = 0.01f; // Very light
        rb.linearDamping = 0.1f; // Slight air resistance
        rb.angularDamping = 0.5f;
        rb.useGravity = useGravity;

        rb.freezeRotation = true;
    }
    
    void FixedUpdate()
    {

        if (useGravity && gravity != 1f && rb != null && !hasHit)
        {
            Vector3 customGravity = Physics.gravity * (gravity - 1f);
            rb.AddForce(customGravity, ForceMode.Acceleration);
        }
    }
    
    void SetupCollider()
    {
        if (col == null)
        {

            col = gameObject.AddComponent<CapsuleCollider>();
        }

        if (col is CapsuleCollider capsule)
        {
            capsule.radius = 0.005f; // Very thin
            capsule.height = 0.08f; // Nail length
            capsule.direction = 2; // Z-axis (forward)
        }

        col.isTrigger = false;
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (hasHit) return;
        
        hasHit = true;

        ContactPoint contact = collision.contacts[0];
        Vector3 impactPoint = contact.point;
        Vector3 impactNormal = contact.normal;

        StickToSurface(collision.transform, impactPoint, impactNormal);

        CreateImpactEffects(impactPoint, impactNormal, collision.collider);

        PlayImpactSound(collision.collider);

        IDamageable damageable = collision.collider.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage);
        }
    }
    
    void StickToSurface(Transform surface, Vector3 point, Vector3 normal)
    {

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        transform.SetParent(surface);

        transform.position = point;

        transform.rotation = Quaternion.LookRotation(-normal);

        transform.position += normal * -0.02f;
    }
    
    void CreateImpactEffects(Vector3 point, Vector3 normal, Collider hitCollider)
    {

        GameObject effectPrefab = impactEffectPrefab;

        string materialType = DetermineMaterialType(hitCollider);
        
        if (effectPrefab != null)
        {
            GameObject effect = Instantiate(effectPrefab, point, Quaternion.LookRotation(normal));

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

            Destroy(effect, 2f);
        }
    }
    
    void PlayImpactSound(Collider hitCollider)
    {
        if (impactSounds == null || impactSounds.Length == 0) return;

        string materialType = DetermineMaterialType(hitCollider);
        
        AudioClip soundToPlay = impactSounds[0]; // Default


        soundToPlay = impactSounds[Random.Range(0, impactSounds.Length)];

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

        if (hitCollider.material != null)
        {
            if (hitCollider.material == metalMaterial) return "metal";
            if (hitCollider.material == woodMaterial) return "wood";
            if (hitCollider.material == concreteMaterial) return "concrete";
        }

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

    public void RemoveNail()
    {

        if (impactEffectPrefab != null)
        {
            GameObject effect = Instantiate(impactEffectPrefab, transform.position, transform.rotation);
            Destroy(effect, 1f);
        }
        
        Destroy(gameObject);
    }
}

public interface IDamageable
{
    void TakeDamage(float damage);
}