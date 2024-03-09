using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
 * CS6457 Attributions
 * Tiny Brain
 * Original Author:     Tom
 * Contributors:        
 *
 * External
 * Source Credit:       https://www.youtube.com/watch?v=6-0zD9Xyu5c&ab_channel=SasquatchBStudios
 *                      https://www.youtube.com/watch?v=5L9ksCs6MbE&ab_channel=Unity
 */

public class SceneController : MonoBehaviour
{
    // Singleton Instance 
    private static SceneController _sceneController;
    public GameObject loadingScreen;
    public Image loadingBarFill;

    public static SceneController Instance
    {
        get
        {
            if (!_sceneController)
            {
                _sceneController = FindObjectOfType(typeof(SceneController)) as SceneController;

                if (!_sceneController)
                {
                    Debug.LogError("There needs to be one active SceneController script on a GameObject in your scene.");
                }
            }

            return _sceneController;
        }
    }

    // Inspector Visible Variables
    [Tooltip("The scene that is persistent across the lifetime of the game.")]
    [SerializeField] private SceneAsset _persistentScene;

    [SerializeField]
    [Tooltip("The active (additively loaded) scene.")]
    private SceneAsset _activeScene;

    [Space]

    [Header("Cycle Scenes with F11")]
    [Tooltip("List of available scenes. The first scene is loaded by default. These can be cycled with F11.")]
    [SerializeField] private List<SceneAsset> _scenePlaylist = new();

    [Space]
    [Header("Resetable ScriptableObjects with Init on scene Active/reload")]
    [Tooltip("List of Scriptable Objects that call their Init() function on scene active/reload.")]
    [SerializeField] private List<ScriptableObject> _scriptableObjectsWithInit = new();

    [Space] 
    [Header("Events")] 
    [SerializeField] private GameEvent _sceneLoadComplete; 

    // Local Variables
    private int _currentSceneIndex = 0;

    private Scene _globalParentScene;
    private GameObject _globalParentSceneCamera;

    private GameObject _activeCamera = null;

    private AsyncOperation _sceneChangeOperation = null;

    private bool _sceneUnloadCoroutineComplete = true;
    private bool _sceneLoadCoroutineComplete = true;

    [Header("Global Scene Camera Overridden?")]
    [SerializeField] private bool _overridePersistentSceneCamera = false;

    // Use this for initialization
    void Start()
    {
        Debug.Log("SceneController started");

        if (_persistentScene == null)
        {
            Debug.LogError("Please assign a persistence scene to the SceneController.");
            return;
        }

        if (_scenePlaylist.Any() == false)
        {
            Debug.LogError("Please add at least one scene to the scene cycle.");
            return;
        }

        // Get a reference to the global scene and its camera
        _globalParentScene = SceneManager.GetSceneByName(_persistentScene.name);

        foreach (GameObject gameObject in _globalParentScene.GetRootGameObjects())
        {
            if (gameObject.GetComponent<Camera>() != null)
            {
                _globalParentSceneCamera = gameObject;
                break;
            }
        }

        _activeCamera = _globalParentSceneCamera;

        // Unload all scenes from the hierarchy as they may not match
        // the scene playlist. 
        UnloadAllHierarchyScenesAsync();

        // Load the first scene in the playlist
        ChangeSceneAdditiveAsync(_scenePlaylist[0], null);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F11))
        {
            IncrementPlaylistScene();
        }
        //else if (Input.GetKeyDown(KeyCode.F12))
        //{
        //    SetActiveScene("Assets/Scenes/Tom/AudioManagerTest.unity");
        //}
    }

    public bool ChangeScene(string scenePath, bool removePlaylistHead = true)
    {
        // Insert the scene at the head of the playlist and jump to it
        SceneAsset sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);
        if (sceneAsset == null)
        {
            Debug.LogWarning($"{this.name}: Couldn't load SceneAsset for scene with name \"{scenePath}\"");
            return false;
        }
        
        _scenePlaylist.Insert(0, sceneAsset);
        _currentSceneIndex = -1;
        IncrementPlaylistScene();

        if (removePlaylistHead)
        {
            // Remove the scene asset that was previously
            // at the head of the playlist
            _scenePlaylist.RemoveAt(1);
        }

        return true;
    }

    private void IncrementPlaylistScene()
    {
        if (_scenePlaylist.Count > 1)
        {
            _currentSceneIndex += 1;
            if (_currentSceneIndex >= _scenePlaylist.Count)
                _currentSceneIndex = 0;

            ChangeSceneAdditiveAsync(_scenePlaylist[_currentSceneIndex], _activeScene);
        }
    }

    private void UnloadAllHierarchyScenesAsync()
    {
        int sceneCount = SceneManager.sceneCount;

        for (int i = 0; i < sceneCount; i++)
        {
            Scene hierarchyScene = SceneManager.GetSceneAt(i);

            if (hierarchyScene != _globalParentScene)
                SceneManager.UnloadSceneAsync(hierarchyScene);
        }
    }

    private bool ChangeSceneAdditiveAsync(SceneAsset sceneToLoad, SceneAsset sceneToUnload)
    {
        // We can't start another scene load while another is in flight
        if (_sceneChangeOperation != null)
        {
            Debug.LogWarning($"Cannot change to scene {sceneToLoad.name}. Another scene change is currently in progress.");
            return false;
        }

        // Pop up the loading screen
        loadingScreen.SetActive(true);

        // Kick off the coroutine that will change scenes 
        StartCoroutine(SceneChangeCoroutine(sceneToLoad, sceneToUnload));
        return true;
    }

    private IEnumerator SceneChangeCoroutine(SceneAsset sceneToLoad, SceneAsset sceneToUnload)
    {
        if (sceneToUnload != null)
        {
            _sceneUnloadCoroutineComplete = false;
            StartCoroutine(UnloadSceneCoroutine(sceneToUnload));
            while (_sceneUnloadCoroutineComplete == false)
                yield return null;
        }

        if (sceneToLoad != null)
        {
            _sceneLoadCoroutineComplete = false;
            StartCoroutine(LoadSceneCoroutine(sceneToLoad));
            while (_sceneLoadCoroutineComplete == false)
                yield return null;
        }

        PostSceneLoad(); 
    }

    private IEnumerator UnloadSceneCoroutine(SceneAsset sceneToUnload)
    {
        _sceneChangeOperation = SceneManager.UnloadSceneAsync(sceneToUnload.name);

        while (_sceneChangeOperation.isDone == false)
            yield return false;

        _sceneChangeOperation = null;
        _sceneUnloadCoroutineComplete = true;
    }

    private IEnumerator LoadSceneCoroutine(SceneAsset sceneToLoad)
    {
        Debug.Assert(_sceneChangeOperation == null,
            "LoadSceneCoroutine called while _sceneChangeOperation non null. Cannot load " +
            "a new scene while another is unloading."); 
        
        _sceneChangeOperation = SceneManager.LoadSceneAsync(sceneToLoad.name, LoadSceneMode.Additive);

        while (_sceneChangeOperation.isDone == false)
        {
            // Debug.Log($"{sceneToLoad.name} is loading. Progress {_sceneLoadingOperation.progress * 100}%");
            float progress = Mathf.Clamp01(_sceneChangeOperation.progress / 0.9f);
            loadingBarFill.fillAmount = progress;
            yield return null;
        }

        _activeScene = sceneToLoad;
        _sceneChangeOperation = null;
        _sceneLoadCoroutineComplete = true;
    }

    private void PostSceneLoad()
    {
        // Debug.Log("Post Scene Load");
        Scene scene = SceneManager.GetSceneByName(_activeScene.name);

        _overridePersistentSceneCamera = false;
        foreach (GameObject gameObject in scene.GetRootGameObjects())
        {
            // Try to locate a camera in the additively loaded scene
            if (gameObject.GetComponent<Camera>() == true)
            {
                Debug.Log($"SceneController: Found camera in {_activeScene.name}");
                _overridePersistentSceneCamera = true;
                _activeCamera = gameObject;
            }

            // Ensure that only the global parent scene's EventSystem is active
            if (gameObject.GetComponent<EventSystem>())
            {
                gameObject.SetActive(false);
            }
        }

        // Update global scene camera active state
        if (_overridePersistentSceneCamera)
        {
            _globalParentSceneCamera.SetActive(false);
        }
        else
        {
            _globalParentSceneCamera.SetActive(true);
            _activeCamera = _globalParentSceneCamera;
        }

        // re-initialize Scriptable Objects
        ReInitScriptableObjects();

        // Make the scene active 
        SceneManager.SetActiveScene(scene);
        _sceneLoadComplete.Raise(this);

        // Hide the loading screen 
        loadingScreen.SetActive(false);
    }

    // Start is called before the first frame update
    void ReInitScriptableObjects()
    {
        foreach (ScriptableObjectWithInit so in _scriptableObjectsWithInit)
        {
            so.Init();
        }
    }

    public GameObject GetActiveCameraGameObject()
    {
        return _activeCamera;
    }

    public void SetActiveCamera(GameObject camera)
    {
        // Check to see if we are enabling the global 
        // scene camera (unlikely to ever happen)
        if (camera != _globalParentSceneCamera)
            _overridePersistentSceneCamera = true;
        else
            _overridePersistentSceneCamera = false; 
        
        // Disable the last camera that was active
        _activeCamera.SetActive(false);

        // Enable the new camera
        _activeCamera = camera;
        _activeCamera.SetActive(true);
    }

    public void RestartScene()
    {
        Scene activeScene = SceneManager.GetActiveScene();
        ChangeScene(activeScene.path); 
    }
}