using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO's/FootstepCollection")]
public class FootStepCollection : ScriptableObject
{
    // data for footstep tracking and sound event emitter
    public FloorCharacteristic floorCharacteristic;
    public StepCharacteristic stepCharacteristic;
    public AudioClip[] AudioClips;

    public AudioClip getRandomAudioClip()
    {
        return AudioClips[Random.Range(0, AudioClips.Length)];
    }

}


