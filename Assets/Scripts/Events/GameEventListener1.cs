using UnityEngine;
using UnityEngine.Events;

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

// Generic Single Parameter GameEvent Listener
[AddComponentMenu("Generic 1 Param Event Listener")]
public class GameEventListener1 : GameEventListenerBase, I1ParamEventListener<object>
{
    [System.Serializable]
    public class CustomGameEvent : UnityEvent<Component, object> { }

    [Header("Response Method To Invoke:")]
    [Tooltip("Response to invoke when Event with GameData is raised.")]
    public CustomGameEvent response;

    public void OnEventRaised(object param1)
    {
        response.Invoke(null, param1);
    }
}
