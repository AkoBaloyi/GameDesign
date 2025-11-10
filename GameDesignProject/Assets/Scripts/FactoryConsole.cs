using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;

/// <summary>
/// Console that player activates for second objective
/// Uses F key to interact
/// </summary>
public class FactoryConsole : MonoBehaviour
{
	[Header("References")]
	public ObjectiveManager objectiveManager;
	public InspectionHintUI hintUI;
	
	[Header("UI")]
	public GameObject promptUI;
	public TextMeshProUGUI promptText;
	public string promptMessage = "Press F to Activate Console";
	
	[Header("Visual Feedback")]
	public GameObject screenDisplay;
	public Material inactiveMaterial;
	public Material activeMaterial;
	public Renderer consoleRenderer;
	public Light consoleLight;
	public ParticleSystem activationEffect;
	
	[Header("Audio")]
	public AudioSource audioSource;
	public AudioClip activationSound;
	public AudioClip successSound;
	
	[Header("Detection")]
	public float detectionRange = 3f;
	public LayerMask playerLayer;
	
	[Header("Activation")]
	public float activationDuration = 2f;
	public bool requiresPowerCellFirst = true;
	
	private bool playerInRange = false;
	private bool isActivated = false;
	private bool canActivate = false;

	private void Awake()
	{
		if (objectiveManager == null)
			objectiveManager = FindObjectOfType<ObjectiveManager>();
			
		// Hide prompt initially
		if (promptUI != null)
			promptUI.SetActive(false);
			
		// Set inactive state
		if (consoleRenderer != null && inactiveMaterial != null)
			consoleRenderer.material = inactiveMaterial;
			
		if (consoleLight != null)
			consoleLight.enabled = false;
	}

	private void FixedUpdate()
	{
		// Check for player in range
		Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange, playerLayer);
		
		bool wasInRange = playerInRange;
		playerInRange = colliders.Length > 0;
		
		// Update prompt visibility
		UpdatePrompt();
	}

	private void Update()
	{
		// TEMPORARY DEBUG: Press C to force enable console
		if (Keyboard.current != null && Keyboard.current.cKey.wasPressedThisFrame)
		{
			Debug.Log("[FactoryConsole] DEBUG: Force enabling console with C key");
			EnableConsole();
		}

		// Direct F-key detection as fallback (in case Input System isn't wired up)
		if (playerInRange && !isActivated && canActivate)
		{
			if (Keyboard.current != null && Keyboard.current.fKey.wasPressedThisFrame)
			{
				Debug.Log("[FactoryConsole] F key pressed! Activating console...");
				ActivateConsole();
			}
		}
		else if (playerInRange && !canActivate)
		{
			// DEBUG: Show why F key isn't working
			if (Keyboard.current != null && Keyboard.current.fKey.wasPressedThisFrame)
			{
				Debug.LogWarning("[FactoryConsole] F key pressed but console is NOT enabled yet! canActivate = false");
				Debug.LogWarning("[FactoryConsole] Console must be enabled by LightsController first!");
			}
		}
	}

	private void UpdatePrompt()
	{
		if (isActivated)
		{
			// Already activated, hide prompt
			if (promptUI != null)
				promptUI.SetActive(false);
			return;
		}
		
		// Show prompt if player is in range and console can be activated
		bool shouldShowPrompt = playerInRange && canActivate;
		
		if (promptUI != null)
		{
			promptUI.SetActive(shouldShowPrompt);
			
			if (shouldShowPrompt && promptText != null)
			{
				promptText.text = promptMessage;
			}
		}
	}

	/// <summary>
	/// Enable console for activation (called when lights are activated)
	/// </summary>
	public void EnableConsole()
	{
		canActivate = true;
		Debug.Log("[FactoryConsole] Console enabled for activation");
		
		// Turn on console light to indicate it's ready
		if (consoleLight != null)
		{
			consoleLight.enabled = true;
			consoleLight.color = Color.yellow;
		}
		

	}

	/// <summary>
	/// Called by Input System when F key is pressed
	/// </summary>
	public void OnInteract(InputAction.CallbackContext context)
	{
		if (!context.performed) return;
		if (!playerInRange || isActivated || !canActivate) return;
		
		ActivateConsole();
	}

	/// <summary>
	/// Activate the console
	/// </summary>
	public void ActivateConsole()
	{
		if (isActivated || !canActivate) return;
		
		Debug.Log("[FactoryConsole] Activating console...");
		
		isActivated = true;
		
		// Notify objective manager immediately
		if (objectiveManager != null)
		{
			objectiveManager.OnConsoleInteract();
		}
		
		// Hide prompt
		if (promptUI != null)
			promptUI.SetActive(false);
		
		// Start activation sequence
		StartCoroutine(ActivationSequence());
	}

	private IEnumerator ActivationSequence()
	{
		Debug.Log("[FactoryConsole] Starting activation sequence...");
		
		// Play activation sound
		if (audioSource != null && activationSound != null)
		{
			audioSource.PlayOneShot(activationSound);
		}
		
		// Dramatic light sequence
		if (consoleLight != null)
		{
			// Flash yellow
			consoleLight.color = Color.yellow;
			consoleLight.intensity = 3f;
			yield return new WaitForSeconds(0.2f);
			
			// Flash white
			consoleLight.color = Color.white;
			consoleLight.intensity = 5f;
			yield return new WaitForSeconds(0.1f);
			
			// Settle to green
			float elapsed = 0f;
			while (elapsed < 1f)
			{
				consoleLight.color = Color.Lerp(Color.white, Color.green, elapsed);
				consoleLight.intensity = Mathf.Lerp(5f, 2f, elapsed);
				elapsed += Time.deltaTime;
				yield return null;
			}
			consoleLight.color = Color.green;
			consoleLight.intensity = 2f;
		}
		
		// Wait for activation duration
		yield return new WaitForSeconds(activationDuration);
		
		// Play particle effect
		if (activationEffect != null)
		{
			activationEffect.Play();
		}
		
		// Change material to active (make it glow!)
		if (consoleRenderer != null && activeMaterial != null)
		{
			consoleRenderer.material = activeMaterial;
			
			// Enable emission
			activeMaterial.EnableKeyword("_EMISSION");
			activeMaterial.SetColor("_EmissionColor", Color.green * 2f);
		}
		
		// Play success sound
		if (audioSource != null && successSound != null)
		{
			audioSource.PlayOneShot(successSound);
		}
		
		yield return new WaitForSeconds(0.5f);
		
		// Notify objective manager of completion
		if (objectiveManager != null)
		{
			objectiveManager.OnConsoleActivatedComplete();
		}
		
		// ALSO notify ClearObjectiveManager if it exists
		ClearObjectiveManager clearManager = FindObjectOfType<ClearObjectiveManager>();
		if (clearManager != null)
		{
			clearManager.OnConsoleActivated();
			Debug.Log("[FactoryConsole] Notified ClearObjectiveManager!");
		}
		
		// Hide hint after completion
		if (hintUI != null)
		{
			hintUI.HideHint();
		}
		
		Debug.Log("[FactoryConsole] Console activated successfully!");
	}

	private void OnDrawGizmosSelected()
	{
		// Draw detection range
		Gizmos.color = isActivated ? Color.green : (canActivate ? Color.yellow : Color.red);
		Gizmos.DrawWireSphere(transform.position, detectionRange);
	}
}
