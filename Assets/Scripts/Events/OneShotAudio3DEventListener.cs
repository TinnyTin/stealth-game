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

[AddComponentMenu("One-Shot 3D Audio Event Listener")]
public class OneShotAudio3DEventListener : GameEventListenerBase, I3ParamEventListener<AudioClip, Vector3, AudioSourceParams>
{
    [System.Serializable]
    public class OneShotAudio3DEvent : UnityEvent<AudioClip, Vector3, AudioSourceParams> { }

    [Header("Response Method To Invoke:")]
    [Tooltip("Response to invoke when Event with GameData is raised.")]
    public OneShotAudio3DEvent response;

    public void OnEventRaised(AudioClip audioClip, Vector3 position, AudioSourceParams audioSourceParams)
    {
        response.Invoke(audioClip, position, audioSourceParams);
    }
}
