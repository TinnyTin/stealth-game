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
    private ThreatMeter tm;
    private AIStateMachine ai;

    // Start is called before the first frame update
    void Start()
    {
        tm = GetComponent<ThreatMeter>();
        ai = GetComponent<AIStateMachine>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void onSound3DStringInGameCreated(AudioClip sfx, Vector3 position, string str, AudioSourceParams audioSourceParams)
    {
        // check string is correct
        if (str == "SoundThreat")
        {
            if (Vector3.Distance(position, this.transform.position) <= radiusListenable)
            {
                float val = baseThreatPerSound * weightThreatOfSound;
                tm.changeThreat(val);
                ai.lastThreat = position;
            }
        }
    }



        public void onSoundThreatCreated(Vector3 position, float weightThreatOfSound)
        {
            if (Vector3.Distance(position, this.transform.position) <= radiusListenable)
            {
                float val = baseThreatPerSound * weightThreatOfSound;
                tm.changeThreat(val);
                ai.lastThreat = position;
            }

        }
    }
