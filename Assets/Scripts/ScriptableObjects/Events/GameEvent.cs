using System.Collections.Generic;
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

[CreateAssetMenu(menuName = "SO's/GameEvent")]
public class GameEvent : ScriptableObject
{
    // Event Listeners by parameter count
    [Header("Registered Listeners")]
    public List<GameEventListenerBase> listeners = new();

    // Raise event through different method signatures
    public void Raise()
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            if (listeners[i] is IEventListener listener)
            {
                listener.OnEventRaised();
            }
        }
    }

    public void Raise<T>(T param1)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            if (listeners[i] is I1ParamEventListener<T> listener)
            {
                listener.OnEventRaised(param1);
            }
        }
    }

    public void Raise<T1, T2>(T1 param1, T2 param2)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            if (listeners[i] is I2ParamEventListener<T1, T2> listener)
            {
                listener.OnEventRaised(param1, param2);
            }
        }
    }

    public void Raise<T1, T2, T3>(T1 param1, T2 param2, T3 param3)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            if (listeners[i] is I3ParamEventListener<T1, T2, T3> listener)
            {
                listener.OnEventRaised(param1, param2, param3);
            }
        }
    }

    public void Raise<T1, T2, T3, T4>(T1 param1, T2 param2, T3 param3, T4 param4)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            if (listeners[i] is I4ParamEventListener<T1, T2, T3, T4> listener)
            {
                listener.OnEventRaised(param1, param2, param3, param4);
            }
        }
    }

    // Manage Listeners
    // ############################################################
    public void RegisterListener(GameEventListenerBase listener)
    {
        if (!listeners.Contains(listener))
            listeners.Add(listener);
    }

    public void UnregisterListener(GameEventListenerBase listener)
    {
        if (listeners.Contains(listener))
            listeners.Remove(listener);
    }
}
