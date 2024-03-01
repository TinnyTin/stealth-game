﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

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
                //else
                //{
                //    _sceneController.Init();
                //}
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

    // Local Variables
    private int _currentSceneIndex = 0;

    private Scene _globalParentScene;
    private GameObject _globalParentSceneCamera;

    private GameObject _activeCamera = null; 

    private AsyncOperation _sceneLoadingOperation = null;

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
        LoadSceneAdditiveAsync(_scenePlaylist[0], null);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F11))
        {
            IncrementPlaylistScene();
        }
    }

    private void IncrementPlaylistScene()
    {
        if (_scenePlaylist.Count > 1)
        {
            _currentSceneIndex += 1;
            if (_currentSceneIndex >= _scenePlaylist.Count)
                _currentSceneIndex = 0;

            LoadSceneAdditiveAsync(_scenePlaylist[_currentSceneIndex], _activeScene);
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

    private bool LoadSceneAdditiveAsync(SceneAsset sceneToLoad, SceneAsset sceneToUnload)
    {
        // We can't start another scene load while another is in flight
        if (_sceneLoadingOperation != null)
        {
            Debug.LogWarning($"Cannot load scene {sceneToLoad.name}. Another scene load is currently in progress.");
            return false;
        }

        _sceneLoadingOperation = SceneManager.LoadSceneAsync(sceneToLoad.name, LoadSceneMode.Additive);
        StartCoroutine(LoadSceneCoroutine(sceneToLoad, sceneToUnload)); 
        return true; 
    }

    private IEnumerator LoadSceneCoroutine(SceneAsset sceneToLoad, SceneAsset sceneToUnload)
    {
        while (_sceneLoadingOperation.isDone == false)
        {
            Debug.Log($"{sceneToLoad.name} is loading. Progress {_sceneLoadingOperation.progress * 100}%");
            yield return null;
        }

        _activeScene = sceneToLoad;
        _sceneLoadingOperation = null;

        if (sceneToUnload != null)
            SceneManager.UnloadSceneAsync(sceneToUnload.name);

        // Run the post scene load operations
        PostSceneLoad();
    }

    private void PostSceneLoad()
    {
        Debug.Log("Post Scene Load"); 
        Scene scene = SceneManager.GetSceneByName(_activeScene.name);

        _overridePersistentSceneCamera = false; 
        foreach (GameObject gameObject in scene.GetRootGameObjects())
        {
            // Try to locate a camera in the additvely loaded scene
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

        SceneManager.SetActiveScene(scene);
    }

    public GameObject GetActiveCameraGameObject()
    {
        return _activeCamera; 
    }
}