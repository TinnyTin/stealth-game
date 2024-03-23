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
    [Header("Physics")]
    public float impulseThreshold = 5f;
    [Header("Events")]
    public GameEvent soundEventToRaise;
    public GameEvent soundThreatEvent;
    public float threatWeight = 1f;


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
            if (AudioClipCollision != null)
            {
                // raise sound event
                soundEventToRaise.Raise(AudioClipCollision, transform.position, "SoundThreat", AudioSourceParams.Default);
                // raise threat increase event
                soundThreatEvent.Raise(transform.position, threatWeight);

            }
                
        }
    }
}
