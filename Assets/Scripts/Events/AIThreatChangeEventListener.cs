using UnityEngine;
using UnityEngine.Events;
/*
 * CS6457 Attributions
 * Tiny Brain
 * Original Author:     Justin Wu
 * Contributors:
 * Description: AI Threat change event
 * External
 * Source Credit:       
 *                      
 */
public class AIThreatChangeEventListener : GameEventListenerBase, I1ParamEventListener<AIThreatPriority>
{
    [System.Serializable]
    public class AIThreatChangeEvent: UnityEvent<Component, AIThreatPriority> { }

    [Header("Response Method To Invoke:")]
    [Tooltip("Response to invoke when Event with GameData is raised.")]
    public AIThreatChangeEvent response;

    public void OnEventRaised(AIThreatPriority threatPriority)
    {
        response.Invoke(null, threatPriority);
    }
}
