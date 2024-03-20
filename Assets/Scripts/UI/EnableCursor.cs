using UnityEngine;

/*
 * CS6457 Attributions
 * Tiny Brain
 * Original Author:     Martin
 * Contributors:        
 * 
 * Enables the cursor when the script is enabled
 */
public class EnableCursor : MonoBehaviour
{
    void OnEnable()
    {
        GameManager.Instance.SetCursorVisible(true);
    }
}
