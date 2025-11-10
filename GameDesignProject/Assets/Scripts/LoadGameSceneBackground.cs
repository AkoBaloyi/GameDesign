using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Loads Game scene additively to show MenuBackgroundCamera
/// Disables player and gameplay for background only
/// </summary>
public class LoadGameSceneBackground : MonoBehaviour
{
    public string gameSceneName = "Game";
    
    void Start()
    {
        StartCoroutine(LoadSceneAndDisableGameplay());
    }
    
    IEnumerator LoadSceneAndDisableGameplay()
    {
        // Load Game scene in background
        if (!SceneManager.GetSceneByName(gameSceneName).isLoaded)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(gameSceneName, LoadSceneMode.Additive);
            yield return asyncLoad;
            
            Debug.Log("[LoadGameSceneBackground] Game scene loaded, disabling gameplay...");
            
            Scene gameScene = SceneManager.GetSceneByName(gameSceneName);
            
            // Disable ALL gameplay - only keep camera, lights, and environment
            foreach (GameObject obj in gameScene.GetRootGameObjects())
            {
                string objName = obj.name.ToLower();
                
                // DISABLE these completely
                if (objName.Contains("player") || 
                    objName.Contains("enemy") ||
                    objName.Contains("manager") ||
                    objName.Contains("canvas") ||
                    objName.Contains("eventsystem") ||
                    objName.Contains("spawner") ||
                    objName.Contains("ui"))
                {
                    obj.SetActive(false);
                    Debug.Log($"[LoadGameSceneBackground] DISABLED: {obj.name}");
                    continue;
                }
                
                // KEEP these
                if (objName.Contains("camera") || 
                    objName.Contains("light") ||
                    objName.Contains("floor") ||
                    objName.Contains("wall") ||
                    objName.Contains("environment") ||
                    objName.Contains("factory"))
                {
                    obj.SetActive(true);
                    
                    // Also disable player controller scripts if they exist
                    MonoBehaviour[] scripts = obj.GetComponentsInChildren<MonoBehaviour>();
                    foreach (MonoBehaviour script in scripts)
                    {
                        if (script != null && 
                            (script.GetType().Name.Contains("Player") ||
                             script.GetType().Name.Contains("Controller") ||
                             script.GetType().Name.Contains("Input")))
                        {
                            script.enabled = false;
                            Debug.Log($"[LoadGameSceneBackground] Disabled script: {script.GetType().Name}");
                        }
                    }
                }
            }
            
            // CRITICAL: Disable mouse look on MenuBackgroundCamera
            GameObject bgCamera = GameObject.Find("MenuBackgroundCamera");
            if (bgCamera != null)
            {
                MonoBehaviour[] cameraScripts = bgCamera.GetComponents<MonoBehaviour>();
                foreach (MonoBehaviour script in cameraScripts)
                {
                    if (script != null && script.GetType() != typeof(Camera))
                    {
                        script.enabled = false;
                        Debug.Log($"[LoadGameSceneBackground] Disabled camera script: {script.GetType().Name}");
                    }
                }
            }
            
            Debug.Log("[LoadGameSceneBackground] Background setup complete!");
        }
    }
}
