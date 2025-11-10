using UnityEngine;




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

		if (cellRenderer != null)
		{
			cellRenderer.material.color = cellColor;
			cellRenderer.material.EnableKeyword("_EMISSION");
			cellRenderer.material.SetColor("_EmissionColor", cellColor * 0.5f);
		}
	}
	
	private void Start()
	{

		startPosition = transform.position;
	}

	private void Update()
	{
		if (!isPickedUp)
		{

			transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
			
			float newY = startPosition.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
			transform.position = new Vector3(startPosition.x, newY, startPosition.z);

			if (pointLight != null)
			{
				float pulse = Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
				pointLight.intensity = 2f + pulse;
			}

			if (cellRenderer != null && cellRenderer.material.HasProperty("_EmissionColor"))
			{
				float pulse = Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
				cellRenderer.material.SetColor("_EmissionColor", cellColor * (1f + pulse));
			}
		}
	}



	public void OnPickedUp()
	{
		if (isPickedUp) return;
		
		isPickedUp = true;

		if (audioSource != null && pickupSound != null)
		{
			audioSource.PlayOneShot(pickupSound);
		}

		if (highlightable != null)
		{
			highlightable.HighlightOff();
		}

		if (objectiveManager != null)
		{
			objectiveManager.OnPowerCellPicked();
		}

		ClearObjectiveManager clearManager = FindObjectOfType<ClearObjectiveManager>();
		if (clearManager != null)
		{
			clearManager.OnPowerCellPickedUp();
			Debug.Log("[PowerCell] Notified ClearObjectiveManager!");
		}
		
		Debug.Log("[PowerCell] Picked up! Now find the Power Bay.");
	}



	public bool IsPickedUp()
	{
		return isPickedUp;
	}
}


