using UnityEngine;

public class LevelSelect : MonoBehaviour
{
    public SceneReference Level1;
    public SceneReference MainMenu;
    
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
        SceneController.Instance.ChangeScene(Level1);
    }

    public void returnToMainMenu()
    {
        SceneController.Instance.ChangeScene(MainMenu);
    }

    public void restartLevel()
    {
        SceneController.Instance.RestartScene();
    }
}
