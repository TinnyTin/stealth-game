using UnityEngine;
using UnityEngine.Events;

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
