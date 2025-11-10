using UnityEngine;
using UnityEngine.SceneManagement;



public class MenuCameraView : MonoBehaviour
{
    [Header("Settings")]
    public string gameSceneName = "Game";
    public Camera menuCamera;
    
    [Header("Camera Position")]
    public Vector3 cameraPosition = new Vector3(0, 2, 0);
    public Vector3 cameraRotation = new Vector3(0, 0, 0);
    public float rotationSpeed = 5f; // Slow rotation for cinematic effect
    
    private Scene gameScene;
    private bool gameSceneLoaded = false;

    void Start()
    {

        if (!SceneManager.GetSceneByName(gameSceneName).isLoaded)
        {
            StartCoroutine(LoadGameSceneInBackground());
        }
    }

    System.Collections.IEnumerator LoadGameSceneInBackground()
    {

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(gameSceneName, LoadSceneMode.Additive);
        asyncLoad.allowSceneActivation = true;

        yield return new WaitUntil(() => asyncLoad.isDone);

        gameScene = SceneManager.GetSceneByName(gameSceneName);
        gameSceneLoaded = true;

        Debug.Log("[MenuCameraView] Game scene loaded in background");

        SetupCamera();
    }

    void SetupCamera()
    {
        if (menuCamera == null)
        {
            menuCamera = Camera.main;
        }

        if (menuCamera != null)
        {
            menuCamera.transform.position = cameraPosition;
            menuCamera.transform.eulerAngles = cameraRotation;
            Debug.Log($"[MenuCameraView] Camera positioned at {cameraPosition}");
        }
    }

    void Update()
    {
        if (gameSceneLoaded && menuCamera != null)
        {

            menuCamera.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }

    void OnDestroy()
    {

        if (gameSceneLoaded && gameScene.isLoaded)
        {
            SceneManager.UnloadSceneAsync(gameScene);
            Debug.Log("[MenuCameraView] Game scene unloaded");
        }
    }
}
