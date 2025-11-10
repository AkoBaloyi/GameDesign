using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System.Collections;

public class ObjectiveManager : MonoBehaviour
{
	[Header("UI - Objective Display")]
	public TextMeshProUGUI objectiveText;
	public GameObject objectiveBanner;
	public CanvasGroup bannerCanvasGroup;
	public float bannerDisplayDuration = 3f;
	
	[Header("UI - Tracker")]
	public TextMeshProUGUI trackerText;
	public GameObject trackerPanel;
	
	[Header("Directional Display")]
	public DirectionalObjectiveDisplay directionalDisplay;
	public Transform powerCellTransform;
	public Transform powerBayTransform;
	public Transform consoleTransform;

	[Header("Audio")]
	public AudioSource audioSource;
	public AudioClip advanceObjectiveSfx;

	[Header("Signals / Hooks")]
	public UnityEvent onObjectiveStartRestorePower;   // enable power cell spawner, show prompt, etc.
	public UnityEvent onPowerCellInserted;           // enable lights controller
	public UnityEvent onStartGlowingPath;            // enable glowing floor/path
	public UnityEvent onConsoleActivated;            // prepare win screen
	public UnityEvent onWin;                         // show win screen

	public enum Step
	{
		LearnControls,
		RestorePowerObjective,
		PickupPowerCell,
		InsertPowerCell,
		LightsActivate,
		FollowPathToConsole,
		ActivateConsole,
		Win
	}

	[Header("Debug")]
	public Step currentStep = Step.LearnControls;

	private void Start()
	{

		if (objectiveBanner != null)
		{
			objectiveBanner.SetActive(false);
		}

		if (trackerPanel != null)
		{
			trackerPanel.SetActive(true);
		}
	}

	public void StartObjectives()
	{
		SetStep(Step.LearnControls, "Learn the basic controls");
		UpdateTracker("Tutorial in progress...");
	}

	public void OnTutorialCompleted()
	{
		SetStep(Step.RestorePowerObjective, "OBJECTIVE: Restore Power to the Factory");
		UpdateTracker("Power Cells: 0/1");
		ShowBanner("OBJECTIVE: Restore Power to the Factory");
		onObjectiveStartRestorePower?.Invoke();
	}

	public void OnPowerCellPicked()
	{

		SetStep(Step.InsertPowerCell, "Insert the Power Cell into the Power Bay");
		UpdateTracker("Power Cells: 1/1 - Find Power Bay");

		if (directionalDisplay != null && powerBayTransform != null)
		{
			directionalDisplay.SetTarget(powerBayTransform);
		}
	}

	public void OnPowerCellInserted()
	{
		SetStep(Step.LightsActivate, "Activating lights...");
		UpdateTracker("Power Cell Inserted ✓");
		ShowBanner("Power Cell Inserted!");
		onPowerCellInserted?.Invoke();
	}

	public void OnLightsActivated()
	{
		Debug.Log("[ObjectiveManager] OnLightsActivated() called!");
		SetStep(Step.FollowPathToConsole, "Follow the glowing path to the console");
		UpdateTracker("Follow the path");
		Debug.Log("[ObjectiveManager] Invoking onStartGlowingPath event...");
		onStartGlowingPath?.Invoke();
		Debug.Log("[ObjectiveManager] onStartGlowingPath event invoked!");

		if (directionalDisplay != null && consoleTransform != null)
		{
			directionalDisplay.SetTarget(consoleTransform);
		}
	}

	public void OnConsoleInteract()
	{
		SetStep(Step.ActivateConsole, "Activate the console");
		onConsoleActivated?.Invoke();
	}

	public void OnConsoleActivatedComplete()
	{
		SetStep(Step.Win, "Factory Power Restored!");
		UpdateTracker("Mission Complete!");
		ShowBanner("FACTORY POWER RESTORED!");
		onWin?.Invoke();
	}

	private void SetStep(Step step, string uiText)
	{
		currentStep = step;
		if (objectiveText != null)
		{
			objectiveText.text = uiText;
		}
		PlayAdvanceSfx();
	}

	private void PlayAdvanceSfx()
	{
		if (audioSource != null && advanceObjectiveSfx != null)
		{
			audioSource.PlayOneShot(advanceObjectiveSfx);
		}
	}

	private void UpdateTracker(string text)
	{
		if (trackerText != null)
		{
			trackerText.text = text;
		}
	}

	public void ShowBanner(string message)
	{
		if (objectiveBanner != null)
		{
			StopAllCoroutines();
			StartCoroutine(ShowBannerCoroutine(message));
		}
	}

	private IEnumerator ShowBannerCoroutine(string message)
	{

		if (objectiveText != null)
		{
			objectiveText.text = message;
		}

		objectiveBanner.SetActive(true);

		if (bannerCanvasGroup != null)
		{
			float elapsed = 0f;
			float fadeDuration = 0.5f;
			while (elapsed < fadeDuration)
			{
				bannerCanvasGroup.alpha = Mathf.Lerp(0, 1, elapsed / fadeDuration);
				elapsed += Time.deltaTime;
				yield return null;
			}
			bannerCanvasGroup.alpha = 1;
		}

		yield return new WaitForSeconds(bannerDisplayDuration);

		if (bannerCanvasGroup != null)
		{
			float elapsed = 0f;
			float fadeDuration = 0.5f;
			while (elapsed < fadeDuration)
			{
				bannerCanvasGroup.alpha = Mathf.Lerp(1, 0, elapsed / fadeDuration);
				elapsed += Time.deltaTime;
				yield return null;
			}
			bannerCanvasGroup.alpha = 0;
		}

		objectiveBanner.SetActive(false);
	}
}


