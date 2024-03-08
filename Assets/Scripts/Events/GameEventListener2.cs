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

// Generic 2 Parameter Event Listener
[AddComponentMenu("Generic 2 Param Event Listener")]
public class GameEventListener2 : GameEventListenerBase, I2ParamEventListener<object, object>
{
    [System.Serializable]
    public class CustomGameEvent2 : UnityEvent<Component, object, object> { }

    [Header("Response Method To Invoke:")]
    [Tooltip("Response to invoke when Event with GameData is raised.")]
    public CustomGameEvent2 response;

    public void OnEventRaised(object param1, object param2)
    {
        response.Invoke(null, param1, param1);
    }
}
