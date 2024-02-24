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

// 3 Parameter GameEvent Listener
public class GameEventListener3 : GameEventListenerBase
{
    [System.Serializable]
    public class CustomGameEvent3 : UnityEvent<Component, object, object, object> {}

    [Header("Response Method To Invoke:")]
    [Tooltip("Response to invoke when Event with GameData is raised.")]
    public CustomGameEvent3 response;

    public void OnEventRaised(Component sender, object data1, object data2, object data3) 
    {
        response.Invoke(sender, data1, data2, data3);
    }
}
