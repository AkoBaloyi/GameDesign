using UnityEngine;

public class ConsoleActivator : MonoBehaviour
{
	[Header("Objective Manager")]
	public ObjectiveManager objectiveManager;

	private void Awake()
	{
		if (objectiveManager == null)
			objectiveManager = FindObjectOfType<ObjectiveManager>();
	}

	public void OnConsoleUsed()
	{
		if (objectiveManager != null)
		{
			objectiveManager.OnConsoleInteract();
			objectiveManager.OnConsoleActivatedComplete();
		}
	}
}


