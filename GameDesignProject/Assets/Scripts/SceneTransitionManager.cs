using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

/// <summary>
/// Handles smooth scene transitions with loading screen
/// </summary>
public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance { get; private set; }
    
    [Header("Loading Screen")]
    public GameObject loadingScreen;
    public Slider progressBar;
    public TextMeshProUGUI loadingText;
    public CanvasGroup loadingCanvasGroup;
    
    [Header("Fade Settings")]
    public float fadeDuration = 0.5f;
    
    [Header("Loading Tips")]
    public string[] loadingTips = new string[]
    {
        "Press E to pick up objects",
        "Press F to interact with doors and consoles",
        "Follow the glowing path to your objective",
        "Check your objective tracker in the top corner",
        "Press ESC to pause the game"
    };
    
    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        // Hide loading screen initially
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(false);
        }
    }
    
    /// <summary>
    /// Load a scene by name with loading screen
    /// </summary>
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }
    
    /// <summary>
    /// Load a scene by build index with loading screen
    /// </summary>
    public void LoadScene(int sceneIndex)
    {
        StartCoroutine(LoadSceneAsync(sceneIndex));
    }
    
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        // Show loading screen
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true);
        }
        
        // Fade in loading screen
        yield return StartCoroutine(FadeLoadingScreen(1f));
        
        // Show random loading tip
        if (loadingText != null && loadingTips.Length > 0)
        {
            loadingText.text = loadingTips[Random.Range(0, loadingTips.Length)];
        }
        
        // Start loading
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;
        
        // Update progress bar
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            
            if (progressBar != null)
            {
                progressBar.value = progress;
            }
            
            // Scene is ready
            if (operation.progress >= 0.9f)
            {
                // Wait a moment
                yield return new WaitForSeconds(0.5f);
                
                // Activate scene
                operation.allowSceneActivation = true;
            }
            
            yield return null;
        }
        
        // Fade out loading screen
        yield return StartCoroutine(FadeLoadingScreen(0f));
        
        // Hide loading screen
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(false);
        }
    }
    
    private IEnumerator LoadSceneAsync(int sceneIndex)
    {
        // Show loading screen
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true);
        }
        
        // Fade in loading screen
        yield return StartCoroutine(FadeLoadingScreen(1f));
        
        // Show random loading tip
        if (loadingText != null && loadingTips.Length > 0)
        {
            loadingText.text = loadingTips[Random.Range(0, loadingTips.Length)];
        }
        
        // Start loading
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false;
        
        // Update progress bar
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            
            if (progressBar != null)
            {
                progressBar.value = progress;
            }
            
            // Scene is ready
            if (operation.progress >= 0.9f)
            {
                // Wait a moment
                yield return new WaitForSeconds(0.5f);
                
                // Activate scene
                operation.allowSceneActivation = true;
            }
            
            yield return null;
        }
        
        // Fade out loading screen
        yield return StartCoroutine(FadeLoadingScreen(0f));
        
        // Hide loading screen
        if (loadingScreen != null)
        {
            loadingScreen.SetActive(false);
        }
    }
    
    private IEnumerator FadeLoadingScreen(float targetAlpha)
    {
        if (loadingCanvasGroup == null) yield break;
        
        float startAlpha = loadingCanvasGroup.alpha;
        float elapsed = 0f;
        
        while (elapsed < fadeDuration)
        {
            loadingCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / fadeDuration);
            elapsed += Time.unscaledDeltaTime; // Use unscaled time in case game is paused
            yield return null;
        }
        
        loadingCanvasGroup.alpha = targetAlpha;
    }
    
    /// <summary>
    /// Quick fade to black and load scene (no loading screen)
    /// </summary>
    public void QuickLoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
    /// <summary>
    /// Quick fade to black and load scene (no loading screen)
    /// </summary>
    public void QuickLoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
