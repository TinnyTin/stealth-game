using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO's/Audio/OneShot2DSound")]
public class ScriptOneShot2DSound : ScriptableObjectWithInit
{
    public GameEvent eventToRaise;
    public AudioClip audioClip;
    private bool soundPlayed = false;

    public override void Init()
    {
        ResetFailSoundTrigger();
    }
    public void Play2DSound()
    {
        if (!soundPlayed)
        {
            if (audioClip != null)
                eventToRaise.Raise(audioClip, AudioSourceParams.Default);
            soundPlayed = true;
        }

    }

    public void ResetFailSoundTrigger()
    {
        soundPlayed = false;
    }

}
