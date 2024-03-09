﻿using UnityEngine;
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

[AddComponentMenu("Player Stole Item Listener")]
public class PlayerStoleItemListener : GameEventListenerBase, IEventListener
{
    [System.Serializable]
    public class PlayerStoleItemEvent : UnityEvent<Component> { }

    [Header("Response Method To Invoke:")]
    [Tooltip("Response to invoke when Event with GameData is raised.")]
    public PlayerStoleItemEvent response;

    public void OnEventRaised()
    {
        response.Invoke(null);
    }
}
