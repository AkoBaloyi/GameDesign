using UnityEngine;




public class FactoryAtmosphere : MonoBehaviour
{
    [Header("Dust Particles")]
    public ParticleSystem dustParticles;
    public int dustParticleCount = 100;
    public float dustAreaSize = 20f;
    
    [Header("Steam Effects")]
    public ParticleSystem[] steamVents;
    public float steamInterval = 5f;
    public float steamDuration = 2f;
    
    [Header("Ambient Lighting")]
    public Light[] emergencyLights;
    public Color emergencyColor = new Color(1f, 0.3f, 0f); // Orange
    public float flickerSpeed = 0.5f;
    public float flickerAmount = 0.3f;
    
    [Header("Fog Settings")]
    public bool enableFog = true;
    public Color fogColor = new Color(0.2f, 0.2f, 0.25f);
    public float fogDensity = 0.02f;
    
    private float steamTimer = 0f;

    private void Start()
    {
        SetupDustParticles();
        SetupEmergencyLights();
        SetupFog();
    }

    private void Update()
    {
        UpdateEmergencyLights();
        UpdateSteamVents();
    }

    private void SetupDustParticles()
    {
        if (dustParticles != null)
        {
            var main = dustParticles.main;
            main.maxParticles = dustParticleCount;
            main.startLifetime = 10f;
            main.startSpeed = 0.1f;
            main.startSize = 0.05f;
            main.startColor = new Color(1f, 1f, 1f, 0.3f);
            
            var emission = dustParticles.emission;
            emission.rateOverTime = dustParticleCount / 10f;
            
            var shape = dustParticles.shape;
            shape.shapeType = ParticleSystemShapeType.Box;
            shape.scale = new Vector3(dustAreaSize, dustAreaSize * 0.5f, dustAreaSize);
            
            dustParticles.Play();
            
            Debug.Log("[FactoryAtmosphere] Dust particles setup complete");
        }
    }

    private void SetupEmergencyLights()
    {
        foreach (var light in emergencyLights)
        {
            if (light != null)
            {
                light.color = emergencyColor;
                light.intensity = 1f;
            }
        }
    }

    private void SetupFog()
    {
        if (enableFog)
        {
            RenderSettings.fog = true;
            RenderSettings.fogColor = fogColor;
            RenderSettings.fogDensity = fogDensity;
            RenderSettings.fogMode = FogMode.ExponentialSquared;
            
            Debug.Log("[FactoryAtmosphere] Fog enabled");
        }
    }

    private void UpdateEmergencyLights()
    {

        float flicker = Mathf.PerlinNoise(Time.time * flickerSpeed, 0f);
        float intensity = 1f + (flicker - 0.5f) * flickerAmount;
        
        foreach (var light in emergencyLights)
        {
            if (light != null)
            {
                light.intensity = intensity;
            }
        }
    }

    private void UpdateSteamVents()
    {
        steamTimer += Time.deltaTime;
        
        if (steamTimer >= steamInterval)
        {
            steamTimer = 0f;
            PlayRandomSteamVent();
        }
    }

    private void PlayRandomSteamVent()
    {
        if (steamVents.Length == 0) return;
        
        int randomIndex = Random.Range(0, steamVents.Length);
        ParticleSystem vent = steamVents[randomIndex];
        
        if (vent != null)
        {
            vent.Play();
            Debug.Log($"[FactoryAtmosphere] Steam vent {randomIndex} activated");
        }
    }



    public void SetAtmosphereActive(bool active)
    {
        if (dustParticles != null)
        {
            if (active)
                dustParticles.Play();
            else
                dustParticles.Stop();
        }
        
        foreach (var light in emergencyLights)
        {
            if (light != null)
            {
                light.enabled = active;
            }
        }
    }



    public void IntensifyAtmosphere()
    {
        if (dustParticles != null)
        {
            var emission = dustParticles.emission;
            emission.rateOverTime = dustParticleCount / 5f; // Double the dust
        }
        
        foreach (var light in emergencyLights)
        {
            if (light != null)
            {
                light.intensity *= 1.5f;
            }
        }
        
        RenderSettings.fogDensity = fogDensity * 1.5f;
        
        Debug.Log("[FactoryAtmosphere] Atmosphere intensified!");
    }
}
