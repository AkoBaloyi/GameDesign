using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;




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

		if (promptUI != null)
			promptUI.SetActive(false);

		if (consoleRenderer != null && inactiveMaterial != null)
			consoleRenderer.material = inactiveMaterial;
			
		if (consoleLight != null)
			consoleLight.enabled = false;
	}

	private void FixedUpdate()
	{

		Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange, playerLayer);
		
		bool wasInRange = playerInRange;
		playerInRange = colliders.Length > 0;

		UpdatePrompt();
	}

	private void Update()
	{

		if (Keyboard.current != null && Keyboard.current.cKey.wasPressedThisFrame)
		{
			Debug.Log("[FactoryConsole] DEBUG: Force enabling console with C key");
			EnableConsole();
		}

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

			if (promptUI != null)
				promptUI.SetActive(false);
			return;
		}

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



	public void EnableConsole()
	{
		canActivate = true;
		Debug.Log("[FactoryConsole] Console enabled for activation");

		if (consoleLight != null)
		{
			consoleLight.enabled = true;
			consoleLight.color = Color.yellow;
		}
		

	}



	public void OnInteract(InputAction.CallbackContext context)
	{
		if (!context.performed) return;
		if (!playerInRange || isActivated || !canActivate) return;
		
		ActivateConsole();
	}



	public void ActivateConsole()
	{
		if (isActivated || !canActivate) return;
		
		Debug.Log("[FactoryConsole] Activating console...");
		
		isActivated = true;

		if (objectiveManager != null)
		{
			objectiveManager.OnConsoleInteract();
		}

		if (promptUI != null)
			promptUI.SetActive(false);

		StartCoroutine(ActivationSequence());
	}

	private IEnumerator ActivationSequence()
	{
		Debug.Log("[FactoryConsole] Starting activation sequence...");

		if (audioSource != null && activationSound != null)
		{
			audioSource.PlayOneShot(activationSound);
		}

		if (consoleLight != null)
		{

			consoleLight.color = Color.yellow;
			consoleLight.intensity = 3f;
			yield return new WaitForSeconds(0.2f);

			consoleLight.color = Color.white;
			consoleLight.intensity = 5f;
			yield return new WaitForSeconds(0.1f);

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

		yield return new WaitForSeconds(activationDuration);

		if (activationEffect != null)
		{
			activationEffect.Play();
		}

		if (consoleRenderer != null && activeMaterial != null)
		{
			consoleRenderer.material = activeMaterial;

			activeMaterial.EnableKeyword("_EMISSION");
			activeMaterial.SetColor("_EmissionColor", Color.green * 2f);
		}

		if (audioSource != null && successSound != null)
		{
			audioSource.PlayOneShot(successSound);
		}
		
		yield return new WaitForSeconds(0.5f);

		if (objectiveManager != null)
		{
			objectiveManager.OnConsoleActivatedComplete();
		}

		ClearObjectiveManager clearManager = FindObjectOfType<ClearObjectiveManager>();
		if (clearManager != null)
		{
			clearManager.OnConsoleActivated();
			Debug.Log("[FactoryConsole] Notified ClearObjectiveManager!");
		}

		if (hintUI != null)
		{
			hintUI.HideHint();
		}
		
		Debug.Log("[FactoryConsole] Console activated successfully!");
	}

	private void OnDrawGizmosSelected()
	{

		Gizmos.color = isActivated ? Color.green : (canActivate ? Color.yellow : Color.red);
		Gizmos.DrawWireSphere(transform.position, detectionRange);
	}
}
