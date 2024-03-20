using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * CS6457 Attributions
 * Tiny Brain
 * Original Author:     Martin
 * Contributors:        
 * 
 * Locks cursor to the center of the screen and hides visibility.
 */
public class CursorLock : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        if(gameObject.tag == "Menu")
        {
            lockCursorControl(false);
        }
    }

    public void lockCursorControl(bool enable)
    {

        if (enable)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            return;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }
}
