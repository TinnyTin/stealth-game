using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class SoundEmittingObject : MonoBehaviour
{
  public AudioClip AudioClipCollision;
  public GameEvent eventToRaise;

  // Start is called before the first frame update
  void Start()
  {
    if(AudioClipCollision == null)
    {
      Debug.LogError("SoundEmittingObject: No SoundEmittingObject AudioClip");
    }
  }

  // TODO: make this on fall, and vary sounds
  public void OnCollisionEnter()
  {
    if(AudioClipCollision != null)
      eventToRaise.Raise(AudioClipCollision, transform.position, "SoundThreat");
  }
}
