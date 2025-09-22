using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NailgunWeapon : MonoBehaviour
{
    [Header("Weapon Stats")]
    public int maxAmmo = 50;
    public int currentAmmo = 0;
    public float fireRate = 0.1f; // Time between shots
    public float nailSpeed = 50f;
    public float damage = 10f;
    
    [Header("Projectile")]
    public GameObject nailPrefab;
    public Transform firePoint;
    
    [Header("Visual Effects")]
    public ParticleSystem muzzleFlash;
    public ParticleSystem impactEffect;
    public GameObject sparksEffect;
    
    [Header("Audio")]
    public AudioSource weaponAudio;
    public AudioClip fireSound;
    public AudioClip emptySound;
    public AudioClip reloadSound;
    
    [Header("UI")]
    public TextMeshProUGUI ammoText;
    public Image ammoBar;
    public GameObject crosshair;
    
    [Header("Camera Effects")]
    public float recoilStrength = 0.5f;
    public Transform cameraTransform;
    
    [Header("References")]
    public TutorialManager tutorialManager;
    
    // Private variables
    private bool isEquipped = false;
    private float lastFireTime = 0f;
    private Vector3 originalCameraPos;
    private bool isRecoiling = false;
    
    void Start()
    {
        if (cameraTransform != null)
        {
            originalCameraPos = cameraTransform.localPosition;
        }
        
        UpdateAmmoUI();
        
        // Start with weapon disabled
        gameObject.SetActive(false);
    }
    
    void Update()
    {
        if (!isEquipped) return;
        
        HandleRecoil();
        
        // Handle firing input
        bool fireInput = false;
        
        // Check both mouse and gamepad input
        if (UnityEngine.InputSystem.Mouse.current != null && UnityEngine.InputSystem.Mouse.current.leftButton.wasPressedThisFrame)
        {
            fireInput = true;
        }
        
        if (UnityEngine.InputSystem.Gamepad.current != null && UnityEngine.InputSystem.Gamepad.current.rightTrigger.wasPressedThisFrame)
        {
            fireInput = true;
        }
        
        if (fireInput)
        {
            TryFire();
        }
    }
    
    public void EquipWeapon()
    {
        isEquipped = true;
        gameObject.SetActive(true);
        
        // Show crosshair
        if (crosshair != null)
        {
            crosshair.SetActive(true);
        }
        
        // Notify tutorial
        if (tutorialManager != null)
        {
            tutorialManager.OnNailgunPickedUp();
        }
        
        Debug.Log("Nailgun equipped!");
    }
    
    public void UnequipWeapon()
    {
        isEquipped = false;
        gameObject.SetActive(false);
        
        // Hide crosshair
        if (crosshair != null)
        {
            crosshair.SetActive(false);
        }
    }
    
    void TryFire()
    {
        // Check fire rate
        if (Time.time - lastFireTime < fireRate) return;
        
        // Check ammo
        if (currentAmmo <= 0)
        {
            PlayEmptySound();
            return;
        }
        
        Fire();
        lastFireTime = Time.time;
    }
    
    void Fire()
    {
        // Consume ammo
        currentAmmo--;
        UpdateAmmoUI();
        
        // Create nail projectile
        if (nailPrefab != null && firePoint != null)
        {
            GameObject nail = Instantiate(nailPrefab, firePoint.position, firePoint.rotation);
            
            // Get nail projectile component and fire it
            NailProjectile nailScript = nail.GetComponent<NailProjectile>();
            if (nailScript != null)
            {
                nailScript.damage = damage;
                nailScript.impactEffectPrefab = sparksEffect;
                nailScript.FireNail(firePoint.forward, nailSpeed);
            }
            else
            {
                // Fallback: directly set Rigidbody velocity
                Rigidbody nailRb = nail.GetComponent<Rigidbody>();
                if (nailRb != null)
                {
                    nailRb.linearVelocity = firePoint.forward * nailSpeed;
                }
            }
        }
        
        // Visual effects
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }
        
        // Audio
        if (weaponAudio != null && fireSound != null)
        {
            weaponAudio.PlayOneShot(fireSound);
        }
        
        // Camera recoil
        StartRecoil();
        
        // Notify tutorial
        if (tutorialManager != null)
        {
            tutorialManager.OnNailgunFired();
        }
    }
    
    public void LoadAmmo(int amount)
    {
        int ammoToAdd = Mathf.Min(amount, maxAmmo - currentAmmo);
        currentAmmo += ammoToAdd;
        
        UpdateAmmoUI();
        
        // Play reload sound
        if (weaponAudio != null && reloadSound != null)
        {
            weaponAudio.PlayOneShot(reloadSound);
        }
        
        // Notify tutorial
        if (tutorialManager != null && ammoToAdd > 0)
        {
            tutorialManager.OnNailsLoaded();
        }
        
        Debug.Log($"Loaded {ammoToAdd} nails. Total: {currentAmmo}/{maxAmmo}");
    }
    
    void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            ammoText.text = $"{currentAmmo}/{maxAmmo}";
        }
        
        if (ammoBar != null)
        {
            ammoBar.fillAmount = (float)currentAmmo / maxAmmo;
        }
    }
    
    void StartRecoil()
    {
        if (cameraTransform == null) return;
        
        isRecoiling = true;
        
        // Apply upward recoil
        Vector3 recoilOffset = new Vector3(
            Random.Range(-0.1f, 0.1f), 
            -recoilStrength, 
            Random.Range(-0.05f, 0.05f)
        );
        
        cameraTransform.localPosition = originalCameraPos + recoilOffset;
    }
    
    void HandleRecoil()
    {
        if (!isRecoiling || cameraTransform == null) return;
        
        // Smoothly return camera to original position
        cameraTransform.localPosition = Vector3.Lerp(
            cameraTransform.localPosition, 
            originalCameraPos, 
            Time.deltaTime * 10f
        );
        
        // Stop recoil when close enough
        if (Vector3.Distance(cameraTransform.localPosition, originalCameraPos) < 0.01f)
        {
            cameraTransform.localPosition = originalCameraPos;
            isRecoiling = false;
        }
    }
    
    void PlayEmptySound()
    {
        if (weaponAudio != null && emptySound != null)
        {
            weaponAudio.PlayOneShot(emptySound);
        }
    }
    
    // Public getters for other systems
    public bool IsEquipped() { return isEquipped; }
    public int GetCurrentAmmo() { return currentAmmo; }
    public int GetMaxAmmo() { return maxAmmo; }
    public bool HasAmmo() { return currentAmmo > 0; }
}