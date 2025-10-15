using UnityEngine;

public class PowerBay : MonoBehaviour
{
	[Header("Objective Manager")]
	public ObjectiveManager objectiveManager;

	[Header("Socket")]
	public Transform socketPoint;

	private void Awake()
	{
		if (objectiveManager == null)
			objectiveManager = FindObjectOfType<ObjectiveManager>();
	}

	public void InsertPowerCell(GameObject powerCell)
	{
		if (powerCell == null) return;
		powerCell.transform.SetParent(socketPoint);
		powerCell.transform.position = socketPoint.position;
		powerCell.transform.rotation = socketPoint.rotation;
		var rb = powerCell.GetComponent<Rigidbody>();
		if (rb != null)
		{
			rb.isKinematic = true;
			rb.useGravity = false;
		}
		if (objectiveManager != null)
			objectiveManager.OnPowerCellInserted();
	}
}


