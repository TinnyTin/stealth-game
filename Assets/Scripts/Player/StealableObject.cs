using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class StealableObject : MonoBehaviour
{
  public AudioClip AudioClipSteal;
  public GameEvent eventToRaise;
  public GameObject objectiveTracker;

  private GameObject childDefault, childHilighted;

  // Start is called before the first frame update
  void Start()
  {
    if(AudioClipSteal == null)
    {
      Debug.LogError("StealableObject: No steal AudioClip");
    }

    foreach(Transform child in transform)
    {
      if(child.name == "Default")
        childDefault = child.GameObject();
      else if (child.name == "Hilighted")
        childHilighted = child.GameObject();
    }
    if(childDefault == null || childHilighted == null)
    {
      Debug.LogError("StealableObject: Default, Hilight objects not found.");
    }
    else
    {
      SetHilight(false);
    }
  }

  public void Steal()
  {
    if(AudioClipSteal != null)
      eventToRaise.Raise(this, AudioClipSteal, transform.position);
    if(objectiveTracker != null)
        {
            objectiveTracker.GetComponent<ObjectiveTracker>().mainObjectiveObtained();
            Debug.Log("tracker hit");
        }
  }

  public void SetHilight(bool isHilighted)
  {
    if (childDefault == null || childHilighted == null)
      return;

    if (isHilighted)
    {
      childDefault.SetActive(false);
      childHilighted.SetActive(true);
    }
    else
    {
      childDefault.SetActive(true);
      childHilighted.SetActive(false);
    }
  }
}
