using Unity.VisualScripting;
using UnityEngine;

/*
 * CS6457 Attributions
 * Tiny Brain
 * Original Author:     Tom
 * Contributors:        
 */

public class GameManager : MonoBehaviour
{
    // Singleton Instance 
    private static GameManager _gameManager;

    public static GameManager Instance
    {
        get
        {
            if (!_gameManager)
            {
                _gameManager = FindObjectOfType(typeof(GameManager)) as GameManager;

                if (!_gameManager)
                {
                    Debug.LogError("There needs to be one active SceneController script on a GameObject in your scene.");
                }
            }

            return _gameManager;
        }
    }

    [SerializeField] private PlayerData _playerData;

    [DoNotSerialize] public readonly MissionTimer MissionTimer = new();

    private readonly GameSaveUtil _gameSaveUtil = new();
    private GameSave _gameSave = null;

    // Start is called before the first frame update
    void Start()
    {
        LoadGameSave();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Alpha8))
        //{
        //    TestGameSave();
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha9))
        //{
        //    TestLoadSave();
        //}
    }

    public void OnPlayerGoalCameraPanCompleted(Component sender)
    {
        Debug.Log("Start player timer, etc");
    }

    public void OnPlayerCaught(Component sender)
    {
        Debug.Log("Kleptocrat captured. Off to jail with you!");
    }

    public void OnPlayerStoleItem(Component sender)
    {
        Debug.Log("Kleptocrat has the item.");
    }

    public void OnPlayerReachedExit(Component sender)
    {
        Debug.Log("Kleptocrat has reached an extraction point.");
    }

    public void SetCursorVisible(bool visible)
    {
        Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = visible;
    }

    private void LoadGameSave()
    {
        string saveFileName = "test.sav";
        _gameSave = _gameSaveUtil.LoadSave(saveFileName);
        if (_gameSave == null)
        {
            Debug.LogWarning($"GameManager: Could not load save file with name: {saveFileName}. Creating default game save.");
            _gameSave = new GameSave();
        }
        else
        {
            Debug.Log($"GameManager: Loaded save file {saveFileName} successfully.");
        }
    }

    private void TestGameSave()
    {
        MissionTimer.Stop();

        GameSave gameSave = new();
        gameSave.UnlockedToLevel = 2;

        GameSave.LevelStat levelStat = new();
        levelStat.LevelNumber = 1;
        levelStat.BestTime = MissionTimer.GetLastElapsedTime(); 

        gameSave.LevelStats.Add(levelStat);

        _gameSaveUtil.WriteSave(gameSave, "test.sav");
    }

    private void TestLoadSave()
    {
        GameSave test = _gameSaveUtil.LoadSave("test.sav");
        int a = 0; 
    }
}
