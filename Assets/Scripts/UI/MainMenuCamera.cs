using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCamera : MonoBehaviour
{
    private Animator anim;
    public GameObject levelSelectPanel;
    public GameObject mainMenuPanel;
    private Animator levelSelectAnim;
    private Animator mainMenuAnim;
    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
        levelSelectAnim = levelSelectPanel.GetComponent<Animator>();
        mainMenuAnim = mainMenuPanel.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToLevelSelect()
    {
        anim.SetBool("LevelSelect", true);
        if (levelSelectAnim != null)
        {
            levelSelectAnim.SetBool("LevelSelect", true);
        }
        if (mainMenuAnim != null)
        {
            mainMenuAnim.SetBool("LevelSelect", true);
        }
    }

    public void ReturnToMainMenu()
    {
        anim.SetBool("LevelSelect", false);
        if (levelSelectAnim  != null)
        {
            levelSelectAnim.SetBool("LevelSelect", false);
        }
        if (mainMenuAnim != null)
        {
            mainMenuAnim.SetBool("LevelSelect", false);
        }
    }
}
