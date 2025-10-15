using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class DoorInteractor : MonoBehaviour
{
	[Header("Setup")]
	public Animator doorAnimator;                 // Animator on the door with an "Open" trigger or bool
	public string animatorOpenTrigger = "Open";   // Trigger or bool name
	public bool useBoolToOpen = false;            // If true, set a bool; else set a trigger

	[Header("Prompt UI")]
	public GameObject promptRoot;                 // World-space or screen-space UI container (shown/hidden)
	public TextMeshProUGUI promptText;            // Optional text component for the prompt
	public string promptMessage = "Press E to open";

	[Header("Player Detection")]
	public string playerTag = "Player";          // Player must carry this tag
	public bool requireLineOfSight = false;       // Optional: raycast check if needed
	public Transform rayOrigin;                   // If using LoS, where to raycast from (e.g., player's camera)
	public LayerMask losMask = ~0;                // LoS mask

	[Header("State")] 
	public bool isOpen = false;

	private bool playerInRange = false;

	private void Awake()
	{
		if (promptText != null)
		{
			promptText.text = promptMessage;
		}
		SetPromptVisible(false);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag(playerTag)) return;
		playerInRange = true;
		TryShowPrompt(other.transform);
	}

	private void OnTriggerExit(Collider other)
	{
		if (!other.CompareTag(playerTag)) return;
		playerInRange = false;
		SetPromptVisible(false);
	}

	private void Update()
	{
		if (!playerInRange || isOpen) return;

		// Optional LoS gate
		if (requireLineOfSight && rayOrigin != null)
		{
			if (!HasLineOfSight())
			{
				SetPromptVisible(false);
				return;
			}
		}

		// Display prompt while in range
		SetPromptVisible(true);

		// Input (Unity Input System)
		if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
		{
			OpenDoor();
		}
	}

	private void OpenDoor()
	{
		if (doorAnimator != null)
		{
			if (useBoolToOpen)
			{
				doorAnimator.SetBool(animatorOpenTrigger, true);
			}
			else
			{
				doorAnimator.SetTrigger(animatorOpenTrigger);
			}
		}
		isOpen = true;
		SetPromptVisible(false);
	}

	private void TryShowPrompt(Transform playerTransform)
	{
		if (requireLineOfSight && rayOrigin != null)
		{
			if (!HasLineOfSight()) return;
		}
		SetPromptVisible(true);
	}

	private bool HasLineOfSight()
	{
		Ray ray = new Ray(rayOrigin.position, (transform.position - rayOrigin.position).normalized);
		if (Physics.Raycast(ray, out RaycastHit hit, 100f, losMask))
		{
			return hit.collider.transform == transform || hit.collider.transform.IsChildOf(transform);
		}
		return false;
	}

	private void SetPromptVisible(bool visible)
	{
		if (promptRoot != null)
		{
			promptRoot.SetActive(visible);
		}
	}
}


