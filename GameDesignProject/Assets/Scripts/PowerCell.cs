using UnityEngine;

/// <summary>
/// Power Cell that player picks up and inserts into Power Bay
/// Works with E key pickup system
/// </summary>
[RequireComponent(typeof(HighlightableObject))]
public class PowerCell : MonoBehaviour
{
	[Header("References")]
	public ObjectiveManager objectiveManager;
	public Transform playerHand; // Where to attach when picked up
	
	[Header("Visual Feedback")]
	public GameObject glowEffect;
	public Color cellColor = new Color(1f, 0.5f, 0f); // Orange
	public float rotationSpeed = 30f;
	public float bobSpeed = 1f;
	public float bobHeight = 0.2f;
	
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
		startPosition = transform.position;
		
		// Set orange color
		if (cellRenderer != null)
		{
			cellRenderer.material.color = cellColor;
			cellRenderer.material.EnableKeyword("_EMISSION");
			cellRenderer.material.SetColor("_EmissionColor", cellColor * 0.5f);
		}
	}

	private void Update()
	{
		if (!isPickedUp)
		{
			// Rotate and bob when not picked up
			transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
			
			float newY = startPosition.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
			transform.position = new Vector3(startPosition.x, newY, startPosition.z);
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


