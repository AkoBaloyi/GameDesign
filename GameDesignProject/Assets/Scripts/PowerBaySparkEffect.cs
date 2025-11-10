using UnityEngine;




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

        if (sparkParticles == null)
        {
            sparkParticles = GetComponentInChildren<ParticleSystem>();
        }

        if (flashLight == null)
        {
            flashLight = GetComponentInChildren<Light>();
        }

        if (sparkParticles != null)
        {
            sparkParticles.Stop();
        }
        
        if (flashLight != null)
        {
            flashLight.enabled = false;
        }
    }



    public void PlaySparkEffect()
    {
        if (isPlaying) return;
        
        StartCoroutine(SparkEffectCoroutine());
    }

    private System.Collections.IEnumerator SparkEffectCoroutine()
    {
        isPlaying = true;
        
        Debug.Log("[PowerBaySparkEffect] Playing spark effect!");

        if (audioSource != null && sparkSound != null)
        {
            audioSource.PlayOneShot(sparkSound);
        }

        if (sparkParticles != null)
        {
            var main = sparkParticles.main;
            main.startColor = sparkColor;
            sparkParticles.Play();
        }

        if (flashLight != null)
        {
            flashLight.enabled = true;
            flashLight.color = sparkColor;
            flashLight.intensity = flashIntensity;

            float elapsed = 0f;
            while (elapsed < flashDuration)
            {
                flashLight.intensity = Mathf.Lerp(flashIntensity, 0, elapsed / flashDuration);
                elapsed += Time.deltaTime;
                yield return null;
            }
            
            flashLight.enabled = false;
        }

        yield return new WaitForSeconds(sparkDuration);

        if (sparkParticles != null)
        {
            sparkParticles.Stop();
        }
        
        isPlaying = false;
        
        Debug.Log("[PowerBaySparkEffect] Spark effect complete!");
    }



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
