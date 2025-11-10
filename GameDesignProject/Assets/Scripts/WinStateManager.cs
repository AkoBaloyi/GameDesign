using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;



public class WinStateManager : MonoBehaviour
{
	[Header("UI References")]
	public GameObject winPanel;
	public TextMeshProUGUI titleText;
	public TextMeshProUGUI messageText;
	public Button restartButton;
	public Button quitButton;
	public CanvasGroup panelCanvasGroup;
	
	[Header("Win Screen Content")]
	public string winTitle = "MISSION COMPLETE";
	public string winMessage = "Factory Power Restored!\n\nAll systems operational.";
	
	[Header("Animation")]
	public float fadeDuration = 1f;
	public float delayBeforeShow = 2f;
	
	[Header("Audio")]
	public AudioSource audioSource;
	public AudioClip winMusic;
	public AudioClip winSound;
	
	private void Start()
	{

		if (winPanel != null)
		{
			winPanel.SetActive(false);
		}

		if (restartButton != null)
		{
			restartButton.onClick.AddListener(RestartGame);
		}
		
		if (quitButton != null)
		{
			quitButton.onClick.AddListener(QuitGame);
		}
	}



	public void ShowWinScreen()
	{
		StartCoroutine(ShowWinScreenCoroutine());
	}

	private IEnumerator ShowWinScreenCoroutine()
	{
		Debug.Log("[WinStateManager] Showing win screen...");

		yield return new WaitForSeconds(delayBeforeShow);

		if (audioSource != null && winSound != null)
		{
			audioSource.PlayOneShot(winSound);
		}

		if (titleText != null)
		{
			titleText.text = winTitle;
		}
		
		if (messageText != null)
		{
			messageText.text = winMessage;
		}

		if (winPanel != null)
		{
			winPanel.SetActive(true);
		}

		if (panelCanvasGroup != null)
		{
			float elapsed = 0f;
			while (elapsed < fadeDuration)
			{
				panelCanvasGroup.alpha = Mathf.Lerp(0, 1, elapsed / fadeDuration);
				elapsed += Time.deltaTime;
				yield return null;
			}
			panelCanvasGroup.alpha = 1;
		}

		if (audioSource != null && winMusic != null)
		{
			audioSource.clip = winMusic;
			audioSource.loop = true;
			audioSource.Play();
		}

		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;

		Time.timeScale = 0f;
	}



	public void RestartGame()
	{
		Debug.Log("[WinStateManager] Restarting game...");

		Time.timeScale = 1f;

		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}



	public void QuitGame()
	{
		Debug.Log("[WinStateManager] Quitting game...");

		Time.timeScale = 1f;

		if (SceneManager.sceneCountInBuildSettings > 1)
		{
			SceneManager.LoadScene(0);
		}
		else
		{

			#if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false;
			#else
				Application.Quit();
			#endif
		}
	}
}
