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
        SceneController.Instance.ChangeScene("Assets/Scenes/Levels/Level1.unity");
    }

    public void returnToMainMenu()
    {
        SceneController.Instance.ChangeScene("Assets/Scenes/MainMenu.unity");
    }

    public void restartLevel()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //SceneController.Instance.RestartScene();

        SceneController.Instance.RestartScene();
    }
}
