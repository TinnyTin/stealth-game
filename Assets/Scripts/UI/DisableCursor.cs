using UnityEngine;

/*
 * CS6457 Attributions
 * Tiny Brain
 * Original Author:     Tom
 * Contributors:        
 * 
 * Locks cursor to the center of the screen and hides visibility
 * when the script is enabled. 
 */
public class DisableCursor : MonoBehaviour
{
    void OnEnable()
    {
        GameManager.Instance.SetCursorVisible(false);
    }
}
