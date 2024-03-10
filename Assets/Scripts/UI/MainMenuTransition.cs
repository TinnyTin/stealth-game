using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * CS6457 Attributions
 * Tiny Brain
 * Original Author: Martin Lee
 * Contributors:
 * Description: UI Animation event functions to ensure controller navigation
 * Buttons are deactivated to ensure they cannot be reached once they slide out of screen.
 */

public class MainMenuTransition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void deactivateMenuContents()
    {
        foreach(Transform c in transform)
        {
            c.gameObject.SetActive(false);
        }
    }

    void activateMenuContents()
    {
        foreach (Transform c in transform)
        {
            c.gameObject.SetActive(true);
        }
    }
}
