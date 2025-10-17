using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Manages the win state screen when player completes all objectives
/// </summary>
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
		// Hide win panel initially
		if (winPanel != null)
		{
			winPanel.SetActive(false);
		}
		
		// Setup button listeners
		if (restartButton != null)
		{
			restartButton.onClick.AddListener(RestartGame);
		}
		
		if (quitButton != null)
		{
			quitButton.onClick.AddListener(QuitGame);
		}
	}

	/// <summary>
	/// Show the win screen (called by ObjectiveManager)
	/// </summary>
	public void ShowWinScreen()
	{
		StartCoroutine(ShowWinScreenCoroutine());
	}

	private IEnumerator ShowWinScreenCoroutine()
	{
		Debug.Log("[WinStateManager] Showing win screen...");
		
		// Wait a moment for dramatic effect
		yield return new WaitForSeconds(delayBeforeShow);
		
		// Play win sound
		if (audioSource != null && winSound != null)
		{
			audioSource.PlayOneShot(winSound);
		}
		
		// Set text
		if (titleText != null)
		{
			titleText.text = winTitle;
		}
		
		if (messageText != null)
		{
			messageText.text = winMessage;
		}
		
		// Show panel
		if (winPanel != null)
		{
			winPanel.SetActive(true);
		}
		
		// Fade in
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
		
		// Play win music
		if (audioSource != null && winMusic != null)
		{
			audioSource.clip = winMusic;
			audioSource.loop = true;
			audioSource.Play();
		}
		
		// Unlock cursor for buttons
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		
		// Pause game
		Time.timeScale = 0f;
	}

	/// <summary>
	/// Restart the current scene
	/// </summary>
	public void RestartGame()
	{
		Debug.Log("[WinStateManager] Restarting game...");
		
		// Reset time scale
		Time.timeScale = 1f;
		
		// Reload current scene
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	/// <summary>
	/// Quit to main menu or quit application
	/// </summary>
	public void QuitGame()
	{
		Debug.Log("[WinStateManager] Quitting game...");
		
		// Reset time scale
		Time.timeScale = 1f;
		
		// Try to load main menu (scene index 0)
		if (SceneManager.sceneCountInBuildSettings > 1)
		{
			SceneManager.LoadScene(0);
		}
		else
		{
			// If no main menu, quit application
			#if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false;
			#else
				Application.Quit();
			#endif
		}
	}
}
