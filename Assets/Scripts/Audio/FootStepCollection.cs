using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * CS6457 Attributions
 * Tiny Brain
 * Original Author:     Justin Wu
 * Contributors:
 * Description: Footstep collection SO for organizing footstep sounds
 * External Source Credit: 
 *                      
 */

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


