using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/*
 * CS6457 Attributions
 * Tiny Brain
 * Original Author: Martin Lee
 * Contributors:
 * Description: Pause Menu for HUD
 */
public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject player;
    private PlayerControl playerControl;

    // Start is called before the first frame update
    void Start()
    {
        if (player != null)
            playerControl = player.GetComponent<PlayerControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(pauseMenu.activeSelf == true) {
                playerControl.isPlayerControlEnabled = true;
                pauseMenu.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                return;
            }
            playerControl.isPlayerControlEnabled = false;
            pauseMenu.SetActive(true);
        }
    }
}
