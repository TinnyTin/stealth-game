using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * CS6457 Attributions
 * Tiny Brain
 * Original Author: Martin Lee
 * Contributors:
 * Description: Main Menu animation triggers
 */

public class MainMenuCamera : MonoBehaviour
{
    private Animator anim;
    public GameObject levelSelectPanel;
    public GameObject mainMenuPanel;
    public GameObject startButton;
    public GameObject chapterOneButton;
    private Animator levelSelectAnim;
    private Animator mainMenuAnim;
    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();
        levelSelectAnim = levelSelectPanel.GetComponent<Animator>();
        mainMenuAnim = mainMenuPanel.GetComponent<Animator>();
        startButton.GetComponent<Button>().Select();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GoToLevelSelect()
    {
        chapterOneButton.GetComponent<Button>().Select();
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
        startButton.GetComponent<Button>().Select();
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
