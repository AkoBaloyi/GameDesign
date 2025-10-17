using UnityEngine;
using System.Collections;

/// <summary>
/// Controls factory lights and glowing path activation
/// </summary>
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
	
	[Header("References")]
	public ObjectiveManager objectiveManager;
	public FactoryConsole factoryConsole;

	private void Awake()
	{
		if (objectiveManager == null)
			objectiveManager = FindObjectOfType<ObjectiveManager>();
			
		if (factoryConsole == null)
			factoryConsole = FindObjectOfType<FactoryConsole>();
			
		// Disable all lights initially
		foreach (var light in lights)
		{
			if (light != null) light.enabled = false;
		}
		
		// Hide path segments initially
		foreach (var segment in pathSegments)
		{
			if (segment != null) segment.SetActive(false);
		}
	}

	/// <summary>
	/// Activate all factory lights with sequential effect
	/// </summary>
	public void ActivateLights()
	{
		StartCoroutine(ActivateLightsSequence());
	}

	private IEnumerator ActivateLightsSequence()
	{
		Debug.Log("[LightsController] Activating lights...");
		
		// Play power on sound
		if (audioSource != null && powerOnSfx != null)
		{
			audioSource.PlayOneShot(powerOnSfx);
		}
		
		// Turn on lights one by one
		foreach (var light in lights)
		{
			if (light != null)
			{
				light.enabled = true;
				yield return new WaitForSeconds(lightActivationDelay);
			}
		}
		
		// Play activation VFX
		foreach (var vfx in activationVfx)
		{
			if (vfx != null) vfx.Play();
		}
		
		yield return new WaitForSeconds(1f);
		
		// Notify objective manager
		if (objectiveManager != null)
		{
			objectiveManager.OnLightsActivated();
		}
		
		Debug.Log("[LightsController] Lights activated!");
	}

	/// <summary>
	/// Activate the glowing path to the console
	/// </summary>
	public void ActivateGlowingPath()
	{
		StartCoroutine(ActivatePathSequence());
	}

	private IEnumerator ActivatePathSequence()
	{
		Debug.Log("[LightsController] Activating glowing path...");
		
		// Play path activation sound
		if (audioSource != null && pathActivationSfx != null)
		{
			audioSource.PlayOneShot(pathActivationSfx);
		}
		
		// Activate path segments one by one
		foreach (var segment in pathSegments)
		{
			if (segment != null)
			{
				segment.SetActive(true);
				
				// Apply glowing material if available
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
		
		// Enable the console for interaction
		if (factoryConsole != null)
		{
			factoryConsole.EnableConsole();
		}
		
		Debug.Log("[LightsController] Glowing path activated!");
	}
}



