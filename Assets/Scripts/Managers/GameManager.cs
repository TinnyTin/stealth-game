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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            TestGameSave();
        }
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

    private void TestGameSave()
    {
        MissionTimer.Stop();

        GameSave gameSave = new();
        gameSave.UnlockedToLevel = 2;

        GameSave.LevelStat levelStat = new();
        levelStat.LevelNumber = 1;
        levelStat.BestTime = MissionTimer.GetTimespan(); 

        gameSave.LevelStats.Add(levelStat);

        _gameSaveUtil.WriteSave(gameSave, Application.dataPath, "test.sav");
    }
}
