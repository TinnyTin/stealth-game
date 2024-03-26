using UnityEngine;
using UnityEngine.Events;

/*
 * CS6457 Attributions
 * Tiny Brain
 * Original Author:     Justin
 * Contributors:
 *
 * External
 * Source Credit:       https://www.gamedev.lu/s/EventSystem_v10.zip
 *                      https://www.youtube.com/watch?v=7_dyDmF0Ktw&ab_channel=ThisisGameDev
 */

[AddComponentMenu("Sound Threat Event Listener")]
public class SoundThreatEventListener : GameEventListenerBase, I2ParamEventListener<Vector3, float>
{
    [System.Serializable]
    public class SoundThreatEvent: UnityEvent<Vector3, float> { }

    [Header("Response Method To Invoke:")]
    [Tooltip("Response to invoke when Event with GameData is raised.")]
    public SoundThreatEvent response;

    public void OnEventRaised(Vector3 position, float val)
    {
        response.Invoke(position, val);
    }
}
