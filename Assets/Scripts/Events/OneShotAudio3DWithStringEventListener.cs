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
[AddComponentMenu("One Shot Audio 3D With String Listener")]
public class OneShotAudio3DWithStringEventListener : GameEventListenerBase, I4ParamEventListener<AudioClip, Vector3, string, AudioSourceParams>
{
    [System.Serializable]
    public class OneShotAudio3DWithStringGameEvent3 : UnityEvent<AudioClip, Vector3, string, AudioSourceParams> {}

    [Header("Response Method To Invoke:")]
    [Tooltip("Response to invoke when Event with GameData is raised.")]
    public OneShotAudio3DWithStringGameEvent3 response;

    public void OnEventRaised(AudioClip audioClip, Vector3 position, string str, AudioSourceParams audioSourceParams) 
    {
        response.Invoke(audioClip, position, str, audioSourceParams);
    }
}
