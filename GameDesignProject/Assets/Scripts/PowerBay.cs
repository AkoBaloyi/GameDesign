using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;

/// <summary>
/// Power Bay station where player inserts the power cell
/// Uses F key to interact when player is nearby with power cell
/// </summary>
public class PowerBay : MonoBehaviour
{
	[Header("References")]
	public ObjectiveManager objectiveManager;
	public Transform socketPoint;
	
	[Header("UI")]
	public GameObject promptUI;
	public TextMeshProUGUI promptText;
	public string promptMessage = "Press F to Insert Power Cell";
	
	[Header("Visual Feedback")]
	public GameObject slotIndicator; // Blue circle/marker
	public Material inactiveMaterial;
	public Material activeMaterial;
	public Renderer bayRenderer;
	public ParticleSystem insertEffect;
	public ParticleSystem sparksEffect; // Sparks that stop when power restored
	
	[Header("Audio")]
	public AudioSource audioSource;
	public AudioClip insertSound;
	public AudioClip activationSound;
	
	[Header("Detection")]
	public float detectionRange = 3f;
	public LayerMask playerLayer;
	
	private bool playerInRange = false;
	private bool hasPowerCell = false;
	private bool isActivated = false;
	private FPController playerController;

	private void Awake()
	{
		if (objectiveManager == null)
			objectiveManager = FindObjectOfType<ObjectiveManager>();
			
		// Hide prompt initially
		if (promptUI != null)
			promptUI.SetActive(false);
			
		// Set inactive material
		if (bayRenderer != null && inactiveMaterial != null)
			bayRenderer.material = inactiveMaterial;
	}

	private void FixedUpdate()
	{
		// Check for player in range
		Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange, playerLayer);
		
		bool wasInRange = playerInRange;
		playerInRange = colliders.Length > 0;
		
		// Get player controller
		if (playerInRange && playerController == null && colliders.Length > 0)
		{
			playerController = colliders[0].GetComponent<FPController>();
		}
		
		// Update prompt visibility
		UpdatePrompt();
	}

	private void Update()
	{
		// Direct F-key detection as fallback (in case Input System isn't wired up)
		if (playerInRange && !isActivated && PlayerHasPowerCell())
		{
			if (Keyboard.current != null && Keyboard.current.fKey.wasPressedThisFrame)
			{
				Debug.Log("[PowerBay] F key pressed! Attempting to insert power cell...");
				GameObject heldObject = playerController.GetHeldObject();
				if (heldObject != null)
				{
					InsertPowerCell(heldObject);
				}
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
		
		// Show prompt if player is in range and has power cell
		bool shouldShowPrompt = playerInRange && PlayerHasPowerCell();
		
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
	/// Check if player is holding a power cell
	/// </summary>
	private bool PlayerHasPowerCell()
	{
		if (playerController == null) return false;
		
		// Check if player is holding an object
		GameObject heldObject = playerController.GetHeldObject();
		if (heldObject == null) return false;
		
		// Check if it's a power cell
		PowerCell powerCell = heldObject.GetComponent<PowerCell>();
		return powerCell != null && powerCell.IsPickedUp();
	}

	/// <summary>
	/// Called by Input System when F key is pressed
	/// </summary>
	public void OnInteract(InputAction.CallbackContext context)
	{
		if (!context.performed) return;
		if (!playerInRange || isActivated) return;
		if (!PlayerHasPowerCell()) return;
		
		// Get the power cell from player
		GameObject heldObject = playerController.GetHeldObject();
		if (heldObject != null)
		{
			InsertPowerCell(heldObject);
		}
	}

	/// <summary>
	/// Insert power cell into the bay
	/// </summary>
	public void InsertPowerCell(GameObject powerCellObject)
	{
		if (powerCellObject == null || isActivated) return;
		
		Debug.Log("[PowerBay] Inserting power cell...");
		
		isActivated = true;
		
		// Play insert sound
		if (audioSource != null && insertSound != null)
		{
			audioSource.PlayOneShot(insertSound);
		}
		
		// Move power cell to socket
		powerCellObject.transform.SetParent(socketPoint);
		powerCellObject.transform.localPosition = Vector3.zero;
		powerCellObject.transform.localRotation = Quaternion.identity;
		
		// Disable physics
		var rb = powerCellObject.GetComponent<Rigidbody>();
		if (rb != null)
		{
			rb.isKinematic = true;
			rb.useGravity = false;
		}
		
		// Release from player's hand
		if (playerController != null)
		{
			playerController.DropHeldObject();
		}
		
		// Hide prompt
		if (promptUI != null)
			promptUI.SetActive(false);
		
		// Start activation sequence
		StartCoroutine(ActivationSequence());
	}

	private IEnumerator ActivationSequence()
	{
		yield return new WaitForSeconds(0.5f);
		
		// DRAMATIC SPARK SURGE before stopping
		if (sparksEffect != null)
		{
			var emission = sparksEffect.emission;
			var originalRate = emission.rateOverTime;
			
			// Surge!
			emission.rateOverTime = 100f;
			yield return new WaitForSeconds(0.3f);
			
			// Stop sparks
			sparksEffect.Stop();
			Debug.Log("[PowerBay] Sparks stopped!");
		}
		
		// Play particle effect
		if (insertEffect != null)
		{
			insertEffect.Play();
		}
		
		// Change material to active
		if (bayRenderer != null && activeMaterial != null)
		{
			bayRenderer.material = activeMaterial;
			
			// Make it glow!
			activeMaterial.EnableKeyword("_EMISSION");
			activeMaterial.SetColor("_EmissionColor", Color.cyan * 2f);
		}
		
		// Play activation sound
		if (audioSource != null && activationSound != null)
		{
			audioSource.PlayOneShot(activationSound);
		}
		
		yield return new WaitForSeconds(1f);
		
		// Notify objective manager
		if (objectiveManager != null)
		{
			objectiveManager.OnPowerCellInserted();
		}
		
		// ALSO notify ClearObjectiveManager if it exists
		ClearObjectiveManager clearManager = FindObjectOfType<ClearObjectiveManager>();
		if (clearManager != null)
		{
			clearManager.OnPowerCellInserted();
			Debug.Log("[PowerBay] Notified ClearObjectiveManager!");
		}
		
		Debug.Log("[PowerBay] Power cell inserted successfully!");
	}

	private void OnDrawGizmosSelected()
	{
		// Draw detection range
		Gizmos.color = isActivated ? Color.green : Color.cyan;
		Gizmos.DrawWireSphere(transform.position, detectionRange);
	}
}


