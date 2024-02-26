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

[CreateAssetMenu(menuName="SO's/GameEvent")]
public class GameEvent : ScriptableObject
{
    // Event Listeners by parameter count
    [Header("Registered Listeners")]
    public List<GameEventListenerBase> listeners = new();

    // Raise event through different method signatures
    // ############################################################
    public void Raise() 
    {
        Raise(null, null);
    }

    public void Raise(object data) 
    {
        Raise(null, data);
    }

    public void Raise(Component sender) 
    {
        Raise(sender, null);
    }

    public void Raise(Component sender, object param1)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            if (listeners[i] is GameEventListener listener)
            {
                listener.OnEventRaised(sender, param1);
            }
        }
    }

    public void Raise(Component sender, object param1, object param2)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            if (listeners[i] is GameEventListener2 listener)
            {
                listener.OnEventRaised(sender, param1, param2);
            }
        }
    }

    public void Raise(Component sender, object param1, object param2, object param3)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            if (listeners[i] is GameEventListener3 listener)
            {
                listener.OnEventRaised(sender, param1, param2, param3);
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
