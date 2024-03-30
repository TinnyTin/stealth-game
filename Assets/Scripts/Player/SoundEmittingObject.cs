using UnityEngine;

/*
 * CS6457 Attributions
 * Tiny Brain
 * Original Author:     Jeesoo
 * Contributors:        
 */

public class SoundEmittingObject : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip AudioClipCollision;
    public float delayToNextSound = 1f;
    [Header("Physics")]
    public float impulseThreshold = 5f;
    
    [Header("Events")]
    public GameEvent soundEventToRaise;
    public GameEvent soundThreatEvent;
    public float threatWeight = 1f;

    private float nextSound = 0f;

    // Start is called before the first frame update
    void Start()
    {
        if (AudioClipCollision == null)
        {
            Debug.LogError("SoundEmittingObject: No SoundEmittingObject AudioClip");
        }
    }

    // TODO: make this on fall, and vary sounds
    public void OnCollisionEnter(Collision c)
    {
        if (c.impulse.magnitude > impulseThreshold)
        {
            // make sure we can't spam the sound with infinite collisions per second
            if (Time.time > nextSound)
            {
                // reset nextSound time
                nextSound = Time.time + delayToNextSound;

                // play audio clip
                if (AudioClipCollision != null)
                {
                    // raise sound event
                    soundEventToRaise.Raise(AudioClipCollision, transform.position, AudioSourceParams.Default);
                    // raise threat increase event
                    soundThreatEvent.Raise(transform.position, threatWeight);
                }
            }
            
                
        }
    }
}
