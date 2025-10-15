using UnityEngine;
using TMPro;

public class WinScreenController : MonoBehaviour
{
	public GameObject winPanel;
	public TextMeshProUGUI winText;
	public AudioSource audioSource;
	public AudioClip winSfx;

	public void ShowWin()
	{
		if (winPanel != null) winPanel.SetActive(true);
		if (winText != null) winText.text = "Factory Power Restored!";
		if (audioSource != null && winSfx != null) audioSource.PlayOneShot(winSfx);
	}
}


