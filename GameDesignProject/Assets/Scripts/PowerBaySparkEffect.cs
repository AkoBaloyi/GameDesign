using UnityEngine;

/// <summary>
/// Creates dramatic spark effect when power cell is inserted into power bay
/// Attach to PowerBay GameObject
/// </summary>
public class PowerBaySparkEffect : MonoBehaviour
{
    [Header("Spark Effect")]
    public ParticleSystem sparkParticles;
    public float sparkDuration = 3f;
    public Color sparkColor = new Color(1f, 0.6f, 0f); // Orange
    
    [Header("Light Flash")]
    public Light flashLight;
    public float flashIntensity = 5f;
    public float flashDuration = 0.5f;
    
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip sparkSound;
    
    private bool isPlaying = false;

    private void Awake()
    {
        // Setup spark particles if not assigned
        if (sparkParticles == null)
        {
            sparkParticles = GetComponentInChildren<ParticleSystem>();
        }
        
        // Setup flash light if not assigned
        if (flashLight == null)
        {
            flashLight = GetComponentInChildren<Light>();
        }
        
        // Disable initially
        if (sparkParticles != null)
        {
            sparkParticles.Stop();
        }
        
        if (flashLight != null)
        {
            flashLight.enabled = false;
        }
    }

    /// <summary>
    /// Play spark effect when power cell is inserted
    /// </summary>
    public void PlaySparkEffect()
    {
        if (isPlaying) return;
        
        StartCoroutine(SparkEffectCoroutine());
    }

    private System.Collections.IEnumerator SparkEffectCoroutine()
    {
        isPlaying = true;
        
        Debug.Log("[PowerBaySparkEffect] Playing spark effect!");
        
        // Play spark sound
        if (audioSource != null && sparkSound != null)
        {
            audioSource.PlayOneShot(sparkSound);
        }
        
        // Start particle effect
        if (sparkParticles != null)
        {
            var main = sparkParticles.main;
            main.startColor = sparkColor;
            sparkParticles.Play();
        }
        
        // Flash light effect
        if (flashLight != null)
        {
            flashLight.enabled = true;
            flashLight.color = sparkColor;
            flashLight.intensity = flashIntensity;
            
            // Fade out flash
            float elapsed = 0f;
            while (elapsed < flashDuration)
            {
                flashLight.intensity = Mathf.Lerp(flashIntensity, 0, elapsed / flashDuration);
                elapsed += Time.deltaTime;
                yield return null;
            }
            
            flashLight.enabled = false;
        }
        
        // Wait for spark duration
        yield return new WaitForSeconds(sparkDuration);
        
        // Stop particles
        if (sparkParticles != null)
        {
            sparkParticles.Stop();
        }
        
        isPlaying = false;
        
        Debug.Log("[PowerBaySparkEffect] Spark effect complete!");
    }

    /// <summary>
    /// Stop spark effect immediately
    /// </summary>
    public void StopSparkEffect()
    {
        StopAllCoroutines();
        
        if (sparkParticles != null)
        {
            sparkParticles.Stop();
        }
        
        if (flashLight != null)
        {
            flashLight.enabled = false;
        }
        
        isPlaying = false;
    }
}
