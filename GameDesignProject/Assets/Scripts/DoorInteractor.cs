using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class DoorInteractor : MonoBehaviour
{
	[Header("Setup")]
	public Animator doorAnimator;                 // Animator on the door with an "Open" trigger or bool
	public string animatorOpenTrigger = "Open";   // Trigger or bool name
	public bool useBoolToOpen = false;            // If true, set a bool; else set a trigger
	public string closedStateName = "Closed";     // Name of the closed/idle state in Animator
	public bool resetAnimatorOnStart = true;      // Force-reset to closed on Start

	[Header("Alternative Open (no Animator)")]
	public Transform doorPivot;                   // If no Animator, rotate this pivot
	public Vector3 openLocalEuler = new Vector3(0, 90, 0);
	public float openDuration = 0.35f;

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
	private float openLerpT = 0f;
	private Quaternion startRot;
	private Quaternion targetRot;

	[Header("Pickup Blocking")]
	public FPController playerController;         // Optional: block pickup while interacting
	public float pickupBlockSeconds = 0.1f;       // Prevent E from dropping items when opening

	private void Awake()
	{
		if (promptText != null)
		{
			promptText.text = promptMessage;
		}
		SetPromptVisible(false);
	}

	private void Start()
	{
		// Safety: ensure door starts closed and param is reset
		if (doorAnimator != null && resetAnimatorOnStart)
		{
			if (useBoolToOpen)
			{
				doorAnimator.SetBool(animatorOpenTrigger, false);
			}
			// If you have a named closed state, force Animator to it at time 0
			if (!string.IsNullOrEmpty(closedStateName))
			{
				try { doorAnimator.Play(closedStateName, 0, 0f); } catch { /* ignore if state name invalid */ }
			}
		}
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
			if (playerController != null)
			{
				playerController.BlockPickupFor(pickupBlockSeconds);
			}
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
		else if (doorPivot != null)
		{
			// Rotate door open over time
			startRot = doorPivot.localRotation;
			targetRot = Quaternion.Euler(openLocalEuler);
			StartCoroutine(OpenLerp());
		}
		isOpen = true;
		SetPromptVisible(false);
	}

	private System.Collections.IEnumerator OpenLerp()
	{
		openLerpT = 0f;
		while (openLerpT < 1f)
		{
			openLerpT += Time.deltaTime / Mathf.Max(0.01f, openDuration);
			if (doorPivot != null)
			{
				doorPivot.localRotation = Quaternion.Slerp(startRot, targetRot, Mathf.SmoothStep(0f, 1f, openLerpT));
			}
			yield return null;
		}
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


