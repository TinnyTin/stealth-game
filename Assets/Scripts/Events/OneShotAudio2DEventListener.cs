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

[AddComponentMenu("One-Shot 2D Audio Event Listener")]
public class OneShotAudio2DEventListener : GameEventListenerBase, I1ParamEventListener<AudioClip>
{
    [System.Serializable]
    public class OneShotAudio2DEvent : UnityEvent<Component, AudioClip> { }

    [Header("Response Method To Invoke:")]
    [Tooltip("Response to invoke when Event with GameData is raised.")]
    public OneShotAudio2DEvent response;

    public void OnEventRaised(Component sender, AudioClip audioClip) 
    {
        response.Invoke(sender, audioClip);
    }
}
