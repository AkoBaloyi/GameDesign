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
		// Play activation sound
		if (audioSource != null && activationSound != null)
		{
			audioSource.PlayOneShot(activationSound);
		}
		
		// Change light to green
		if (consoleLight != null)
		{
			consoleLight.color = Color.green;
		}
		
		// Wait for activation duration
		yield return new WaitForSeconds(activationDuration);
		
		// Play particle effect
		if (activationEffect != null)
		{
			activationEffect.Play();
		}
		
		// Change material to active
		if (consoleRenderer != null && activeMaterial != null)
		{
			consoleRenderer.material = activeMaterial;
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
		
		Debug.Log("[FactoryConsole] Console activated successfully!");
	}

	private void OnDrawGizmosSelected()
	{
		// Draw detection range
		Gizmos.color = isActivated ? Color.green : (canActivate ? Color.yellow : Color.red);
		Gizmos.DrawWireSphere(transform.position, detectionRange);
	}
}
