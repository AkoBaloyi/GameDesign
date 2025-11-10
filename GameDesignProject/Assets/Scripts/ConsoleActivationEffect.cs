using UnityEngine;




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

        if (screenRenderer == null)
        {
            screenRenderer = GetComponentInChildren<Renderer>();
        }

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



    public void ActivateConsole()
    {
        if (isActivated) return;
        
        StartCoroutine(ActivationSequence());
    }

    private System.Collections.IEnumerator ActivationSequence()
    {
        isActivated = true;
        
        Debug.Log("[ConsoleActivationEffect] Activating console!");

        if (audioSource != null && bootupSound != null)
        {
            audioSource.PlayOneShot(bootupSound);
        }

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

        if (audioSource != null && activationSound != null)
        {
            audioSource.PlayOneShot(activationSound);
        }

        if (consoleLight != null)
        {
            consoleLight.enabled = true;
            consoleLight.color = activeColor;
            consoleLight.intensity = lightIntensity;
        }

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
