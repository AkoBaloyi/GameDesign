using UnityEngine;
using System.Collections;



public class LightsController : MonoBehaviour
{
	[Header("Lights")]
	public Light[] lights;
	public float lightActivationDelay = 0.2f; // Delay between each light turning on
	
	[Header("Glowing Path")]
	public GameObject[] pathSegments; // Floor tiles or line renderers
	public Material glowingMaterial;
	public float pathActivationDelay = 0.3f;
	
	[Header("Effects")]
	public ParticleSystem[] activationVfx;
	public AudioSource audioSource;
	public AudioClip powerOnSfx;
	public AudioClip pathActivationSfx;
	public AudioSource factoryAmbientSource; // Factory ambient that starts when power restored
	
	[Header("References")]
	public ObjectiveManager objectiveManager;
	public FactoryConsole factoryConsole;

	private void Awake()
	{
		if (objectiveManager == null)
			objectiveManager = FindObjectOfType<ObjectiveManager>();
			
		if (factoryConsole == null)
			factoryConsole = FindObjectOfType<FactoryConsole>();

		foreach (var light in lights)
		{
			if (light != null) light.enabled = false;
		}

		foreach (var segment in pathSegments)
		{
			if (segment != null) segment.SetActive(false);
		}
	}



	public void ActivateLights()
	{
		StartCoroutine(ActivateLightsSequence());
	}

	private IEnumerator ActivateLightsSequence()
	{
		Debug.Log("[LightsController] Activating lights...");
		Debug.Log($"[LightsController] Found {lights.Length} lights to activate");

		if (audioSource != null && powerOnSfx != null)
		{
			audioSource.PlayOneShot(powerOnSfx);
		}

		int activatedCount = 0;
		foreach (var light in lights)
		{
			if (light != null)
			{
				light.enabled = true;
				activatedCount++;
				Debug.Log($"[LightsController] Activated light {activatedCount}/{lights.Length}");
				yield return new WaitForSeconds(lightActivationDelay);
			}
		}
		
		Debug.Log($"[LightsController] All {activatedCount} lights activated!");

		foreach (var vfx in activationVfx)
		{
			if (vfx != null) vfx.Play();
		}

		if (factoryAmbientSource != null)
		{
			factoryAmbientSource.Play();
			Debug.Log("[LightsController] Factory ambient sound started!");
		}
		
		yield return new WaitForSeconds(1f);

		Debug.Log("[LightsController] Notifying ObjectiveManager...");
		if (objectiveManager != null)
		{
			objectiveManager.OnLightsActivated();
			Debug.Log("[LightsController] ObjectiveManager.OnLightsActivated() called!");
		}
		else
		{
			Debug.LogError("[LightsController] objectiveManager is NULL! Cannot notify!");
		}
		
		Debug.Log("[LightsController] Lights activated!");
	}



	public void ActivateGlowingPath()
	{
		StartCoroutine(ActivatePathSequence());
	}

	private IEnumerator ActivatePathSequence()
	{
		Debug.Log("[LightsController] Activating glowing path...");

		if (audioSource != null && pathActivationSfx != null)
		{
			audioSource.PlayOneShot(pathActivationSfx);
		}

		foreach (var segment in pathSegments)
		{
			if (segment != null)
			{
				segment.SetActive(true);

				if (glowingMaterial != null)
				{
					Renderer renderer = segment.GetComponent<Renderer>();
					if (renderer != null)
					{
						renderer.material = glowingMaterial;
					}
				}
				
				yield return new WaitForSeconds(pathActivationDelay);
			}
		}

		if (factoryConsole != null)
		{
			Debug.Log("[LightsController] Enabling factory console...");
			factoryConsole.EnableConsole();
		}
		else
		{
			Debug.LogError("[LightsController] factoryConsole is NULL! Cannot enable console!");
			Debug.LogError("[LightsController] Please assign FactoryConsole GameObject in LightsController Inspector!");
		}
		
		Debug.Log("[LightsController] Glowing path activated!");
	}
}



