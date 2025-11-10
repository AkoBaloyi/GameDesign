using UnityEngine;




public class AtmosphericEffects : MonoBehaviour
{
    [Header("Dust Particles")]
    public bool enableDust = true;
    public int dustParticleCount = 100;
    public float dustAreaSize = 50f;
    public float dustSpeed = 0.5f;

    [Header("Steam Vents")]
    public Transform[] steamVentLocations;
    public float steamEmissionRate = 20f;
    public float steamBurstInterval = 5f;

    [Header("Sparks")]
    public Transform[] sparkLocations;
    public float sparkInterval = 3f;

    private ParticleSystem dustSystem;
    private ParticleSystem[] steamSystems;
    private ParticleSystem[] sparkSystems;

    void Start()
    {
        if (enableDust)
        {
            CreateDustParticles();
        }

        if (steamVentLocations != null && steamVentLocations.Length > 0)
        {
            CreateSteamVents();
        }

        if (sparkLocations != null && sparkLocations.Length > 0)
        {
            CreateSparks();
        }
    }

    void CreateDustParticles()
    {
        GameObject dustObj = new GameObject("Atmospheric Dust");
        dustObj.transform.SetParent(transform);
        dustObj.transform.position = transform.position;

        dustSystem = dustObj.AddComponent<ParticleSystem>();
        var main = dustSystem.main;
        main.startLifetime = 10f;
        main.startSpeed = dustSpeed;
        main.startSize = 0.1f;
        main.startColor = new Color(0.8f, 0.8f, 0.8f, 0.3f);
        main.maxParticles = dustParticleCount;
        main.loop = true;

        var emission = dustSystem.emission;
        emission.rateOverTime = dustParticleCount / 10f;

        var shape = dustSystem.shape;
        shape.shapeType = ParticleSystemShapeType.Box;
        shape.scale = new Vector3(dustAreaSize, dustAreaSize * 0.5f, dustAreaSize);

        var velocityOverLifetime = dustSystem.velocityOverLifetime;
        velocityOverLifetime.enabled = true;
        velocityOverLifetime.x = new ParticleSystem.MinMaxCurve(-0.5f, 0.5f);
        velocityOverLifetime.y = new ParticleSystem.MinMaxCurve(-0.2f, 0.2f);
        velocityOverLifetime.z = new ParticleSystem.MinMaxCurve(-0.5f, 0.5f);

        var renderer = dustSystem.GetComponent<ParticleSystemRenderer>();
        renderer.renderMode = ParticleSystemRenderMode.Billboard;
        renderer.material = new Material(Shader.Find("Universal Render Pipeline/Particles/Unlit"));
    }

    void CreateSteamVents()
    {
        steamSystems = new ParticleSystem[steamVentLocations.Length];

        for (int i = 0; i < steamVentLocations.Length; i++)
        {
            if (steamVentLocations[i] == null) continue;

            GameObject steamObj = new GameObject($"Steam Vent {i}");
            steamObj.transform.SetParent(steamVentLocations[i]);
            steamObj.transform.localPosition = Vector3.zero;

            ParticleSystem steam = steamObj.AddComponent<ParticleSystem>();
            steamSystems[i] = steam;

            var main = steam.main;
            main.startLifetime = 2f;
            main.startSpeed = 3f;
            main.startSize = new ParticleSystem.MinMaxCurve(0.5f, 1.5f);
            main.startColor = new Color(1f, 1f, 1f, 0.5f);
            main.loop = true;

            var emission = steam.emission;
            emission.rateOverTime = steamEmissionRate;

            var shape = steam.shape;
            shape.shapeType = ParticleSystemShapeType.Cone;
            shape.angle = 15f;
            shape.radius = 0.2f;

            var sizeOverLifetime = steam.sizeOverLifetime;
            sizeOverLifetime.enabled = true;
            sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1f, AnimationCurve.Linear(0f, 0.5f, 1f, 2f));

            var colorOverLifetime = steam.colorOverLifetime;
            colorOverLifetime.enabled = true;
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { new GradientColorKey(Color.white, 0f), new GradientColorKey(Color.gray, 1f) },
                new GradientAlphaKey[] { new GradientAlphaKey(0.5f, 0f), new GradientAlphaKey(0f, 1f) }
            );
            colorOverLifetime.color = gradient;
        }

        if (steamSystems.Length > 0)
        {
            InvokeRepeating(nameof(SteamBurst), steamBurstInterval, steamBurstInterval);
        }
    }

    void CreateSparks()
    {
        sparkSystems = new ParticleSystem[sparkLocations.Length];

        for (int i = 0; i < sparkLocations.Length; i++)
        {
            if (sparkLocations[i] == null) continue;

            GameObject sparkObj = new GameObject($"Sparks {i}");
            sparkObj.transform.SetParent(sparkLocations[i]);
            sparkObj.transform.localPosition = Vector3.zero;

            ParticleSystem sparks = sparkObj.AddComponent<ParticleSystem>();
            sparkSystems[i] = sparks;

            var main = sparks.main;
            main.startLifetime = 0.5f;
            main.startSpeed = new ParticleSystem.MinMaxCurve(2f, 5f);
            main.startSize = 0.05f;
            main.startColor = new Color(1f, 0.8f, 0.3f, 1f);
            main.loop = false;
            main.playOnAwake = false;

            var emission = sparks.emission;
            emission.rateOverTime = 0;
            emission.SetBursts(new ParticleSystem.Burst[] {
                new ParticleSystem.Burst(0f, 20, 40)
            });

            var shape = sparks.shape;
            shape.shapeType = ParticleSystemShapeType.Cone;
            shape.angle = 30f;
            shape.radius = 0.1f;

            var velocityOverLifetime = sparks.velocityOverLifetime;
            velocityOverLifetime.enabled = true;
            velocityOverLifetime.y = new ParticleSystem.MinMaxCurve(-5f);

            var colorOverLifetime = sparks.colorOverLifetime;
            colorOverLifetime.enabled = true;
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[] { 
                    new GradientColorKey(new Color(1f, 1f, 0.5f), 0f), 
                    new GradientColorKey(new Color(1f, 0.3f, 0f), 1f) 
                },
                new GradientAlphaKey[] { 
                    new GradientAlphaKey(1f, 0f), 
                    new GradientAlphaKey(0f, 1f) 
                }
            );
            colorOverLifetime.color = gradient;

            Light sparkLight = sparkObj.AddComponent<Light>();
            sparkLight.type = LightType.Point;
            sparkLight.color = new Color(1f, 0.8f, 0.3f);
            sparkLight.range = 3f;
            sparkLight.intensity = 0f;
        }

        if (sparkSystems.Length > 0)
        {
            InvokeRepeating(nameof(TriggerRandomSpark), sparkInterval, sparkInterval);
        }
    }

    void SteamBurst()
    {
        if (steamSystems == null) return;

        int randomIndex = Random.Range(0, steamSystems.Length);
        if (steamSystems[randomIndex] != null)
        {
            var emission = steamSystems[randomIndex].emission;
            emission.rateOverTime = steamEmissionRate * 3f;

            Invoke(nameof(ResetSteam), 1f);
        }
    }

    void ResetSteam()
    {
        if (steamSystems == null) return;

        foreach (var steam in steamSystems)
        {
            if (steam != null)
            {
                var emission = steam.emission;
                emission.rateOverTime = steamEmissionRate;
            }
        }
    }

    void TriggerRandomSpark()
    {
        if (sparkSystems == null) return;

        int randomIndex = Random.Range(0, sparkSystems.Length);
        if (sparkSystems[randomIndex] != null)
        {
            sparkSystems[randomIndex].Play();

            Light sparkLight = sparkSystems[randomIndex].GetComponent<Light>();
            if (sparkLight != null)
            {
                StartCoroutine(FlashLight(sparkLight));
            }
        }
    }

    System.Collections.IEnumerator FlashLight(Light light)
    {
        float duration = 0.2f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            light.intensity = Mathf.Lerp(2f, 0f, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        light.intensity = 0f;
    }
}
