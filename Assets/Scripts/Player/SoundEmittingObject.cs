using UnityEngine;

public class SoundEmittingObject : MonoBehaviour
{
    public AudioClip AudioClipCollision;
    public GameEvent eventToRaise;
    public float impulseThreshold = 5f;

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
                eventToRaise.Raise(AudioClipCollision, transform.position, AudioSourceParams.Default);
        }
    }
}
