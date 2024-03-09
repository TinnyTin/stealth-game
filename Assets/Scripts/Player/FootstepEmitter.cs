using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Footstep emitter class.  Call EmitFootstep() from an 
animator event to play random footsteps from array
of audio clips for walk and run respectively.
*/

public class FootstepEmitter : MonoBehaviour
{
  public AudioClip[] audioClipWalk;
  public AudioClip[] audioClipRun;
  public GameEvent eventToRaise;

  void Awake()
  {
  }

  public void EmitFootstep(bool isRunning)
  {
    if(eventToRaise != null && audioClipRun != null)
    {
      if (isRunning)
      {
        int audioClipIndex = (int)((float)audioClipRun.Length * UnityEngine.Random.value) % audioClipRun.Length;
        eventToRaise.Raise(audioClipRun[audioClipIndex], transform.position);
        //Debug.Log("Raise");
      }
      else
      {
        int audioClipIndex = (int)((float)audioClipWalk.Length * UnityEngine.Random.value) % audioClipWalk.Length;
        eventToRaise.Raise(audioClipWalk[audioClipIndex], transform.position);
      }

    }
  }

}
