using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * CS6457 Attributions
 * Tiny Brain
 * Original Author:     Justin Wu
 * Contributors:
 * Description: Ears for the AI to listen to sound emitting object in the scene
 * External
 * Source Credit:       
 *                      
 */


[RequireComponent(typeof(AIStateMachine))]
[RequireComponent(typeof(ThreatMeter))]
public class AIAudioListener : MonoBehaviour
{
    public float radiusListenable = 10f;
    public float baseThreatPerSound = 50f;
    public float weightThreatOfSound = 1f;
    public float deafenDelayS = 3f;
    public bool isAudioActive = false;
    private ThreatMeter tm;
    private AIStateMachine ai;

    private float currDeafenDelayS;

    // Start is called before the first frame update
    void Start()
    {
        tm = GetComponent<ThreatMeter>();
        ai = GetComponent<AIStateMachine>();
        currDeafenDelayS = deafenDelayS;
    }

    private void Update()
    {
        // for some reason, in Start() I can't launch a coroutine to do a delayed function call. so the workaround is to do a time check here
        currDeafenDelayS -= Time.deltaTime;
        currDeafenDelayS = Mathf.Max(0, currDeafenDelayS);
        if (currDeafenDelayS <= 0 && isAudioActive == false)
        {
            isAudioActive = true;
        }
    }

    private void OnDisable()
    {
        isAudioActive = false;
    }

    public void onSoundThreatCreated(Vector3 position, float weightThreatOfSound)
    {
        if (isAudioActive && (Vector3.Distance(position, this.transform.position) <= radiusListenable))
        {
            float val = baseThreatPerSound * weightThreatOfSound;
            tm.changeThreat(val);
            ai.lastThreat = position;
        }
    }
}
