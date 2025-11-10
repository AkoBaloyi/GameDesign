using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;



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

        if (loadingScreen != null)
        {
            loadingScreen.SetActive(false);
        }
    }



    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }



    public void LoadScene(int sceneIndex)
    {
        StartCoroutine(LoadSceneAsync(sceneIndex));
    }
    
    private IEnumerator LoadSceneAsync(string sceneName)
    {

        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true);
        }

        yield return StartCoroutine(FadeLoadingScreen(1f));

        if (loadingText != null && loadingTips.Length > 0)
        {
            loadingText.text = loadingTips[Random.Range(0, loadingTips.Length)];
        }

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            
            if (progressBar != null)
            {
                progressBar.value = progress;
            }

            if (operation.progress >= 0.9f)
            {

                yield return new WaitForSeconds(0.5f);

                operation.allowSceneActivation = true;
            }
            
            yield return null;
        }

        yield return StartCoroutine(FadeLoadingScreen(0f));

        if (loadingScreen != null)
        {
            loadingScreen.SetActive(false);
        }
    }
    
    private IEnumerator LoadSceneAsync(int sceneIndex)
    {

        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true);
        }

        yield return StartCoroutine(FadeLoadingScreen(1f));

        if (loadingText != null && loadingTips.Length > 0)
        {
            loadingText.text = loadingTips[Random.Range(0, loadingTips.Length)];
        }

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            
            if (progressBar != null)
            {
                progressBar.value = progress;
            }

            if (operation.progress >= 0.9f)
            {

                yield return new WaitForSeconds(0.5f);

                operation.allowSceneActivation = true;
            }
            
            yield return null;
        }

        yield return StartCoroutine(FadeLoadingScreen(0f));

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



    public void QuickLoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }



    public void QuickLoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
