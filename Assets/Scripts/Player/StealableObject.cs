using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*
 * CS6457 Attributions
 * Tiny Brain
 * Original Author:     Erik
 * Contributors:        
 */

public class StealableObject : MonoBehaviour
{
  public AudioClip AudioClipSteal;
  public GameEvent eventToRaise;
  public GameObject objectiveTracker;

  private GameObject childDefault, childHilighted;

  [SerializeField] private List<GameObject> _objectsToDisable; 

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
      if (AudioClipSteal != null)
      {
          AudioSourceParams audioParams = new AudioSourceParams();
          audioParams.Volume = 0.5f; 
          eventToRaise.Raise(AudioClipSteal, audioParams);
      }

      if(objectiveTracker != null)
        {
            objectiveTracker.GetComponent<ObjectiveTracker>().mainObjectiveObtained();
        }

        foreach (GameObject gameObject in _objectsToDisable)
        {
            if (gameObject != null)
            {
                gameObject.SetActive(false);
            }
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
