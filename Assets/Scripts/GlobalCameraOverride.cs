using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalCameraOverride : MonoBehaviour
{
    private Scene _globalScene; 

    // Start is called before the first frame update
    void Start()
    {
        // Disable the camera in the global scene
        _globalScene = SceneManager.GetSceneByBuildIndex(0);
        foreach (GameObject gameObject in _globalScene.GetRootGameObjects())
        {
            if (gameObject.name == "Main Camera")
            {
                gameObject.SetActive(false);
                break; 
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy()
    {
        // Enable the camera in the global scene
        foreach (GameObject gameObject in _globalScene.GetRootGameObjects())
        {
            if (gameObject.name == "Main Camera")
            {
                gameObject.SetActive(true);
                break;
            }
        }
    }
}
