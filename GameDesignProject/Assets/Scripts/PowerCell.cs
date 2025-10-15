using UnityEngine;

public class PowerCell : MonoBehaviour
{
	[Header("Objective Manager")]
	public ObjectiveManager objectiveManager;

	private void Awake()
	{
		if (objectiveManager == null)
			objectiveManager = FindObjectOfType<ObjectiveManager>();
	}

	public void OnPickedUp()
	{
		if (objectiveManager != null)
			objectiveManager.OnPowerCellPicked();
	}
}


