using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        SceneController.Instance.SetActiveScene("Assets/Scenes/Tom/AudioManagerTest.unity");
    }
}
