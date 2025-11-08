using UnityEngine;

/// <summary>
/// Power Cell that player picks up and inserts into Power Bay
/// Works with E key pickup system
/// </summary>
public class PowerCell : MonoBehaviour
{
	[Header("References")]
	public ObjectiveManager objectiveManager;
	public Transform playerHand; // Where to attach when picked up
	
	[Header("Visual Feedback")]
	public GameObject glowEffect;
	public Light pointLight; // Add this for glow
	public Color cellColor = new Color(1f, 0.5f, 0f); // Orange
	public float rotationSpeed = 30f;
	public float bobSpeed = 1f;
	public float bobHeight = 0.2f;
	public float pulseSpeed = 2f;
	public float pulseAmount = 0.3f;
	
	[Header("Audio")]
	public AudioSource audioSource;
	public AudioClip pickupSound;
	
	private HighlightableObject highlightable;
	private bool isPickedUp = false;
	private Vector3 startPosition;
	private Renderer cellRenderer;

	private void Awake()
	{
		if (objectiveManager == null)
			objectiveManager = FindObjectOfType<ObjectiveManager>();
			
		highlightable = GetComponent<HighlightableObject>();
		cellRenderer = GetComponentInChildren<Renderer>();
		
		// Set orange color
		if (cellRenderer != null)
		{
			cellRenderer.material.color = cellColor;
			cellRenderer.material.EnableKeyword("_EMISSION");
			cellRenderer.material.SetColor("_EmissionColor", cellColor * 0.5f);
		}
	}
	
	private void Start()
	{
		// Store initial position for bobbing
		startPosition = transform.position;
	}

	private void Update()
	{
		if (!isPickedUp)
		{
			// Rotate and bob when not picked up
			transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
			
			float newY = startPosition.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
			transform.position = new Vector3(startPosition.x, newY, startPosition.z);
			
			// Pulse the light intensity
			if (pointLight != null)
			{
				float pulse = Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
				pointLight.intensity = 2f + pulse;
			}
			
			// Pulse the emission
			if (cellRenderer != null && cellRenderer.material.HasProperty("_EmissionColor"))
			{
				float pulse = Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
				cellRenderer.material.SetColor("_EmissionColor", cellColor * (1f + pulse));
			}
		}
	}

	/// <summary>
	/// Called when player picks up the power cell with E key
	/// </summary>
	public void OnPickedUp()
	{
		if (isPickedUp) return;
		
		isPickedUp = true;
		
		// Play pickup sound
		if (audioSource != null && pickupSound != null)
		{
			audioSource.PlayOneShot(pickupSound);
		}
		
		// Disable highlight
		if (highlightable != null)
		{
			highlightable.HighlightOff();
		}
		
		// Notify objective manager
		if (objectiveManager != null)
		{
			objectiveManager.OnPowerCellPicked();
		}
		
		Debug.Log("[PowerCell] Picked up! Now find the Power Bay.");
	}
	
	/// <summary>
	/// Check if this power cell has been picked up
	/// </summary>
	public bool IsPickedUp()
	{
		return isPickedUp;
	}
}


