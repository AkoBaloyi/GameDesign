using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class ObjectiveManager : MonoBehaviour
{
	[Header("UI")]
	public TextMeshProUGUI objectiveText;

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

	public void StartObjectives()
	{
		SetStep(Step.LearnControls, "Learn the basic controls");
	}

	public void OnTutorialCompleted()
	{
		SetStep(Step.RestorePowerObjective, "Objective: Restore Power");
		onObjectiveStartRestorePower?.Invoke();
	}

	public void OnPowerCellPicked()
	{
		// From core loop: pick up power cell → insert in Power Bay
		SetStep(Step.InsertPowerCell, "Insert the Power Cell into the Power Bay");
	}

	public void OnPowerCellInserted()
	{
		SetStep(Step.LightsActivate, "Activating lights...");
		onPowerCellInserted?.Invoke();
	}

	public void OnLightsActivated()
	{
		SetStep(Step.FollowPathToConsole, "Follow the glowing path to the console");
		onStartGlowingPath?.Invoke();
	}

	public void OnConsoleInteract()
	{
		SetStep(Step.ActivateConsole, "Activate the console");
		onConsoleActivated?.Invoke();
	}

	public void OnConsoleActivatedComplete()
	{
		SetStep(Step.Win, "Factory Power Restored!");
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
}


