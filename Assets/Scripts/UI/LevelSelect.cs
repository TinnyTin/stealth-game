using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
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
        SceneController.Instance.SetActiveScene("Assets/Scenes/Levels/Level1.unity");
    }

    public void returnToMainMenu()
    {
        SceneController.Instance.SetActiveScene("Assets/Scenes/MainMenu.unity");
    }

    public void restartLevel()
    {
        SceneController.Instance.SetActiveScene(SceneManager.GetActiveScene().path);
    }
}
