using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;




public class PowerBay : MonoBehaviour
{
	[Header("References")]
	public ObjectiveManager objectiveManager;
	public Transform socketPoint;
	public InspectionHintUI hintUI;
	
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

		if (promptUI != null)
			promptUI.SetActive(false);

		if (bayRenderer != null && inactiveMaterial != null)
			bayRenderer.material = inactiveMaterial;
	}

	private void FixedUpdate()
	{

		Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange, playerLayer);
		
		bool wasInRange = playerInRange;
		playerInRange = colliders.Length > 0;

		if (playerInRange && playerController == null && colliders.Length > 0)
		{
			playerController = colliders[0].GetComponent<FPController>();
		}

		UpdatePrompt();
	}

	private void Update()
	{

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

			if (promptUI != null)
				promptUI.SetActive(false);
			return;
		}

		bool shouldShowPrompt = playerInRange && PlayerHasPowerCell();
		
		if (promptUI != null)
		{
			promptUI.SetActive(shouldShowPrompt);
			
			if (shouldShowPrompt && promptText != null)
			{
				promptText.text = promptMessage;

				if (hintUI != null)
				{
					hintUI.ShowPowerBayHint();
				}
			}
		}
	}



	private bool PlayerHasPowerCell()
	{
		if (playerController == null) return false;

		GameObject heldObject = playerController.GetHeldObject();
		if (heldObject == null) return false;

		PowerCell powerCell = heldObject.GetComponent<PowerCell>();
		return powerCell != null && powerCell.IsPickedUp();
	}



	public void OnInteract(InputAction.CallbackContext context)
	{
		if (!context.performed) return;
		if (!playerInRange || isActivated) return;
		if (!PlayerHasPowerCell()) return;

		GameObject heldObject = playerController.GetHeldObject();
		if (heldObject != null)
		{
			InsertPowerCell(heldObject);
		}
	}



	public void InsertPowerCell(GameObject powerCellObject)
	{
		if (powerCellObject == null || isActivated) return;
		
		Debug.Log("[PowerBay] Inserting power cell...");
		
		isActivated = true;

		if (audioSource != null && insertSound != null)
		{
			audioSource.PlayOneShot(insertSound);
		}

		powerCellObject.transform.SetParent(socketPoint);
		powerCellObject.transform.localPosition = Vector3.zero;
		powerCellObject.transform.localRotation = Quaternion.identity;

		var rb = powerCellObject.GetComponent<Rigidbody>();
		if (rb != null)
		{
			rb.isKinematic = true;
			rb.useGravity = false;
		}

		if (playerController != null)
		{
			playerController.DropHeldObject();
		}

		if (promptUI != null)
			promptUI.SetActive(false);

		StartCoroutine(ActivationSequence());
	}

	private IEnumerator ActivationSequence()
	{
		yield return new WaitForSeconds(0.5f);

		if (sparksEffect != null)
		{
			var emission = sparksEffect.emission;
			var originalRate = emission.rateOverTime;

			emission.rateOverTime = 100f;
			yield return new WaitForSeconds(0.3f);

			sparksEffect.Stop();
			Debug.Log("[PowerBay] Sparks stopped!");
		}

		if (insertEffect != null)
		{
			insertEffect.Play();
		}

		if (bayRenderer != null && activeMaterial != null)
		{
			bayRenderer.material = activeMaterial;

			activeMaterial.EnableKeyword("_EMISSION");
			activeMaterial.SetColor("_EmissionColor", Color.cyan * 2f);
		}

		if (audioSource != null && activationSound != null)
		{
			audioSource.PlayOneShot(activationSound);
		}
		
		yield return new WaitForSeconds(1f);

		if (objectiveManager != null)
		{
			objectiveManager.OnPowerCellInserted();
		}

		ClearObjectiveManager clearManager = FindObjectOfType<ClearObjectiveManager>();
		if (clearManager != null)
		{
			clearManager.OnPowerCellInserted();
			Debug.Log("[PowerBay] Notified ClearObjectiveManager!");
		}

		if (hintUI != null)
		{
			hintUI.ShowEnemyWarningHint();
		}
		
		Debug.Log("[PowerBay] Power cell inserted successfully!");
	}

	private void OnDrawGizmosSelected()
	{

		Gizmos.color = isActivated ? Color.green : Color.cyan;
		Gizmos.DrawWireSphere(transform.position, detectionRange);
	}
}


