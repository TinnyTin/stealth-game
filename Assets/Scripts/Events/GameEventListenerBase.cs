using UnityEngine;

/*
 * CS6457 Attributions
 * Tiny Brain
 * Original Author:     Tom
 * Contributors:        
 *
 * External
 * Source Credit:       https://www.gamedev.lu/s/EventSystem_v10.zip
 *                      https://www.youtube.com/watch?v=7_dyDmF0Ktw&ab_channel=ThisisGameDev
 */

// Event Listener Base
public class GameEventListenerBase : MonoBehaviour
{
    [Header("Event To Listen For:")]
    public GameEvent gameEvent;

    [Space] private int _spacer; 

    public void OnEnable() 
    {
        gameEvent.RegisterListener(this);
    }

    public void OnDisable() 
    {
        gameEvent.UnregisterListener(this);
    }
}
