using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;




public class LoadGameSceneBackground : MonoBehaviour
{
    public string gameSceneName = "Game";
    
    void Start()
    {
        StartCoroutine(LoadSceneAndDisableGameplay());
    }
    
    IEnumerator LoadSceneAndDisableGameplay()
    {

        if (!SceneManager.GetSceneByName(gameSceneName).isLoaded)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(gameSceneName, LoadSceneMode.Additive);
            yield return asyncLoad;
            
            Debug.Log("[LoadGameSceneBackground] Game scene loaded, disabling gameplay...");
            
            Scene gameScene = SceneManager.GetSceneByName(gameSceneName);

            foreach (GameObject obj in gameScene.GetRootGameObjects())
            {
                string objName = obj.name.ToLower();

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

                if (objName.Contains("camera") || 
                    objName.Contains("light") ||
                    objName.Contains("floor") ||
                    objName.Contains("wall") ||
                    objName.Contains("environment") ||
                    objName.Contains("factory"))
                {
                    obj.SetActive(true);

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
