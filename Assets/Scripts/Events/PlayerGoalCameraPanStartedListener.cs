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

[AddComponentMenu("Player Goal Camera Pan Started Listener")]
public class PlayerGoalCameraPanStartedListener : GameEventListenerBase, IEventListener
{
    [System.Serializable]
    public class PlayerGoalCameraPanStartedEvent : UnityEvent<Component> { }

    [Header("Response Method To Invoke:")]
    [Tooltip("Response to invoke when Event with GameData is raised.")]
    public PlayerGoalCameraPanStartedEvent response;

    public void OnEventRaised(Component sender)
    {
        response.Invoke(sender);
    }
}
