using UnityEngine;

/// <summary>
/// Creates visual feedback when factory console is activated
/// Attach to FactoryConsole GameObject
/// </summary>
public class ConsoleActivationEffect : MonoBehaviour
{
    [Header("Screen Glow")]
    public Renderer screenRenderer;
    public Material screenMaterial;
    public Color inactiveColor = Color.black;
    public Color activeColor = Color.green;
    public float glowIntensity = 2f;
    public float transitionDuration = 1f;
    
    [Header("Particle Effect")]
    public ParticleSystem activationParticles;
    public Color particleColor = Color.green;
    
    [Header("Light Effect")]
    public Light consoleLight;
    public float lightIntensity = 3f;
    
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip activationSound;
    public AudioClip bootupSound;
    
    private bool isActivated = false;

    private void Awake()
    {
        // Get screen renderer if not assigned
        if (screenRenderer == null)
        {
            screenRenderer = GetComponentInChildren<Renderer>();
        }
        
        // Setup initial state
        SetScreenColor(inactiveColor, 0f);
        
        if (consoleLight != null)
        {
            consoleLight.enabled = false;
        }
        
        if (activationParticles != null)
        {
            activationParticles.Stop();
        }
    }

    /// <summary>
    /// Activate console with visual effects
    /// </summary>
    public void ActivateConsole()
    {
        if (isActivated) return;
        
        StartCoroutine(ActivationSequence());
    }

    private System.Collections.IEnumerator ActivationSequence()
    {
        isActivated = true;
        
        Debug.Log("[ConsoleActivationEffect] Activating console!");
        
        // Play bootup sound
        if (audioSource != null && bootupSound != null)
        {
            audioSource.PlayOneShot(bootupSound);
        }
        
        // Transition screen color from black to green
        float elapsed = 0f;
        while (elapsed < transitionDuration)
        {
            float t = elapsed / transitionDuration;
            Color currentColor = Color.Lerp(inactiveColor, activeColor, t);
            float currentGlow = Mathf.Lerp(0f, glowIntensity, t);
            SetScreenColor(currentColor, currentGlow);
            
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        SetScreenColor(activeColor, glowIntensity);
        
        // Play activation sound
        if (audioSource != null && activationSound != null)
        {
            audioSource.PlayOneShot(activationSound);
        }
        
        // Enable console light
        if (consoleLight != null)
        {
            consoleLight.enabled = true;
            consoleLight.color = activeColor;
            consoleLight.intensity = lightIntensity;
        }
        
        // Play particle effect
        if (activationParticles != null)
        {
            var main = activationParticles.main;
            main.startColor = particleColor;
            activationParticles.Play();
        }
        
        Debug.Log("[ConsoleActivationEffect] Console activated!");
    }

    private void SetScreenColor(Color color, float emissionIntensity)
    {
        if (screenRenderer != null)
        {
            Material mat = screenRenderer.material;
            mat.color = color;
            
            if (mat.HasProperty("_EmissionColor"))
            {
                mat.EnableKeyword("_EMISSION");
                mat.SetColor("_EmissionColor", color * emissionIntensity);
            }
        }
    }

    /// <summary>
    /// Pulse the screen for attention
    /// </summary>
    public void PulseScreen()
    {
        StartCoroutine(PulseCoroutine());
    }

    private System.Collections.IEnumerator PulseCoroutine()
    {
        float pulseDuration = 0.5f;
        float elapsed = 0f;
        
        while (elapsed < pulseDuration)
        {
            float t = Mathf.Sin(elapsed / pulseDuration * Mathf.PI);
            float currentGlow = Mathf.Lerp(glowIntensity, glowIntensity * 2f, t);
            SetScreenColor(activeColor, currentGlow);
            
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        SetScreenColor(activeColor, glowIntensity);
    }
}
