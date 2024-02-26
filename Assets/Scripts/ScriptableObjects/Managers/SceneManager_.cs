using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * CS6457 Attributions
 * Tiny Brain
 * Original Author:     Tom     
 * Contributors:        
 *
 * External
 * Source Credit:
 */

[CreateAssetMenu(menuName = "SO's/Managers/SceneSwitcher")]
public class SceneManager_ : ScriptableObjectWithStart
{
    public string activeScene = string.Empty;
    private int _currentSceneIndex = 0;

    private Scene _globalScene;
    private GameObject _globalSceneCameraParent; 

    [Space]

    [SerializeField]
    [Tooltip("List of active scenes. Cycle order will be identical to array order.")] 
    private SceneAsset[] _scenes;

    public override void Start()
    {
        Debug.Log("SceneManager_ started");

        if (!_scenes.Any())
        {
            Debug.LogError("Please add at least one scene to the SceneManager_ S.O.");
        }
        else
        {
            // Get a reference to the global scene and its camera
            _globalScene = SceneManager.GetSceneByBuildIndex(0);

            foreach (GameObject gameObject in _globalScene.GetRootGameObjects())
            {
                if (gameObject.GetComponent<Camera>() != null)
                {
                    _globalSceneCameraParent = gameObject;
                    break; 
                }
            }

            // Reset any saved state since the previous execution
            activeScene = string.Empty;
            _currentSceneIndex = 0;

            UnloadAllScenes();
            LoadScene(_currentSceneIndex);
        }
    }

    public void OnEnable()
    {
       
    }

    public void Awake()
    {
        
    }

    private void SetActiveScene(int sceneIndex)
    {
        foreach (SceneAsset scene in _scenes)
        {
            Scene s = SceneManager.GetSceneByName(scene.name);
            SceneManager.SetActiveScene(s); 
        }
    }

    public void SwitchScene(Component sender, object data)
    {
        //Debug.Log("SwitchScene fired");

        if (_scenes.Length > 1)
        {
            UnloadScene(_currentSceneIndex);

            _currentSceneIndex++;
            if (_currentSceneIndex > _scenes.Length - 1)
                _currentSceneIndex = 0;

            LoadScene(_currentSceneIndex);
        }
    }

    private void LoadAllScenes()
    {
        for (int i = 1; i < SceneManager.sceneCount; i++)
        {
            Scene s = SceneManager.GetSceneAt(i);
            SceneManager.LoadScene(s.name, LoadSceneMode.Additive);
        }
    }

    private void UnloadAllScenes()
    {
        int sceneCount = SceneManager.sceneCount;
        for (int i = 1; i < sceneCount; i++)
        {
            Scene s = SceneManager.GetSceneAt(i);
            SceneManager.UnloadSceneAsync(s); 
        }
    }

    private void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(_scenes[sceneIndex].name, LoadSceneMode.Additive);
        activeScene = _scenes[sceneIndex].name;

        //Scene test = SceneManager.GetSceneByName(_scenes[sceneIndex].name);
        //foreach (GameObject gameObject in test.GetRootGameObjects())
        //{
        //    if (gameObject.GetComponent<Camera>() == true)
        //    {
        //        Debug.LogWarning($"Got camera from {gameObject.name}");
        //    }
        //}

        //if (_scenes[sceneIndex].GetComponent<Camera>() != null)
        //{
        //    Debug.LogWarning("Disabling global scene camera");
        //    _globalSceneCameraParent.SetActive(false);
        //}
        //else
        //{
        //    Debug.LogWarning("Enabling global scene camera");
        //    _globalSceneCameraParent.SetActive(true);
        //}
    }

    private void UnloadScene(int sceneIndex)
    {
        SceneManager.UnloadSceneAsync(_scenes[sceneIndex].name); 
    }
}
