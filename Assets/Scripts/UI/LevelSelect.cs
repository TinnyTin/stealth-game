using UnityEngine;

/*
 * CS6457 Attributions
 * Tiny Brain
 * Original Author: Martin Lee
 * Contributors:
 * Description: Scene switching for UI navigation
 */

public class LevelSelect : MonoBehaviour
{
    [SerializeField] 
    private PlayerData _playerData; 

    [Header("Scene Names")]
    [SerializeField] private string _mainMenuSceneName = "MainMenu";
    [SerializeField] private string _level1SceneName = "Level1"; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeLevels()
    {
        SceneController.Instance.ChangeScene(_level1SceneName);
    }

    public void returnToMainMenu()
    {
        SceneController.Instance.ChangeScene(_mainMenuSceneName);
    }

    public void restartLevel()
    {
        if (_playerData != null)
            SceneController.Instance.RestartScene(_playerData.LastCheckpoint);
        else
            SceneController.Instance.RestartScene();
    }
}
