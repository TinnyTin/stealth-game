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
public class OneShotAudio3DWithStringEventListener : GameEventListenerBase, I3ParamEventListener<AudioClip, Vector3, string>
{
    [System.Serializable]
    public class OneShotAudio3DWithStringGameEvent3 : UnityEvent<Component, AudioClip, Vector3, string> {}

    [Header("Response Method To Invoke:")]
    [Tooltip("Response to invoke when Event with GameData is raised.")]
    public OneShotAudio3DWithStringGameEvent3 response;

    public void OnEventRaised(AudioClip param1, Vector3 param2, string param3) 
    {
        response.Invoke(null, param1, param2, param3);
    }
}
