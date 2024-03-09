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

// Generic 3 Parameter Event Listener
[AddComponentMenu("Generic 3 Param Event Listener")]
public class GameEventListener3 : GameEventListenerBase, I3ParamEventListener<object, object, object>
{
    [System.Serializable]
    public class CustomGameEvent3 : UnityEvent<Component, object, object, object> { }

    [Header("Response Method To Invoke:")]
    [Tooltip("Response to invoke when Event with GameData is raised.")]
    public CustomGameEvent3 response;

    public void OnEventRaised(object param1, object param2, object param3)
    {
        response.Invoke(null, param1, param2, param3);
    }
}
