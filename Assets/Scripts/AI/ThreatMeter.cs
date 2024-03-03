using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


[RequireComponent(typeof(AIStateMachine))]
public class ThreatMeter : MonoBehaviour
{
    public Slider threatBar;
    public float minThreatMeter = 0;
    public float maxThreatMeter = 100;
    [Tooltip("Rate of increase in threat on sight")]
    public float sightThreatRate = 10;
    [Tooltip("Multiplier on anything that increases threat")]
    public float NeuroticMultiplier = 1;
    [Tooltip("Rate in threat per second")]
    public float DegredationRate = 10;

    [Header("Current threat level")]
    public float threatLevel = 0;
    public int BehaviorIdx;

    [Header("Behaviors")]
    public List<ThreatThreshold> ThreatBehaviorList;

    private AIStateMachine ai;
    private void Start()
    {
        ai = GetComponent<AIStateMachine>();
        // invoke first behavior. usually idle
        alignBehaviorType();
        ThreatBehaviorList[BehaviorIdx].onThreatReach.Invoke();
    }

    private void FixedUpdate()
    {
        changeThreat(-(DegredationRate * Time.deltaTime));
    }

    public void onSightThreat(Transform t, float interval)
    {
        ai.lastThreat = new Vector3(t.position.x, t.position.y, t.position.z);
        changeThreat(sightThreatRate * interval * NeuroticMultiplier);

    }
    public void changeThreat(float val)
    {
        if (val > 0)
        {
            threatLevel = Mathf.Clamp(threatLevel + val * NeuroticMultiplier, minThreatMeter, maxThreatMeter);
        }
        else
        {
            threatLevel = Mathf.Clamp(threatLevel + val, minThreatMeter, maxThreatMeter);
        }

        onThreatChange();
    }

    private void onThreatChange()
    {
        // check if you have reached a new max.
        alignBehaviorType();
        // update threatBar
        threatBar.value = threatLevel / maxThreatMeter;

    }

    private void alignBehaviorType()
    {
        int lastBhvrIdx = BehaviorIdx;
        // check there is a higher behavior state to reach, and that the AI's threat has exceeded it
        while ((BehaviorIdx + 1 < ThreatBehaviorList.Count) && (threatLevel > ThreatBehaviorList[BehaviorIdx+1].max))
        {
            BehaviorIdx++; // change to higher behavior
        }
        // check if threat degradation moves us to lower behavior state
        while ((BehaviorIdx - 1 >= 0) && (threatLevel <= ThreatBehaviorList[BehaviorIdx].min))
        {
            BehaviorIdx--;
        }
        if (lastBhvrIdx != BehaviorIdx)
        {
            Debug.Log("threat: " + threatLevel);
            ThreatBehaviorList[BehaviorIdx].onThreatReach.Invoke(); // invoke 
        }
    }




    [System.Serializable]
    public struct ThreatThreshold
    {
        public string name;
        public float min;
        public float max;
        public UnityEvent onThreatReach;
    }

}

