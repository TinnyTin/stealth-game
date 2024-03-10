using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class ExtractionPoint : MonoBehaviour
{
  public AudioClip AudioClipExtract;
  public GameEvent eventToRaise;

  private GameObject childDefault, childHilighted;

    // Temporary referencing structure until SO's are setup
    public GameObject gameplayObject;

  // Start is called before the first frame update
  void Start()
  {
    if(AudioClipExtract == null)
    {
      Debug.LogError("ExtractionPoint: No AudioClipExtract");
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

  public void Extract()
  {
    if(AudioClipExtract != null)
      eventToRaise.Raise(AudioClipExtract, transform.position, AudioSourceParams.Default);

    // Temporary until SO's are setup. Raise a "Player Reached Exit" SO event in the future.
    if(gameplayObject != null)
        {
            gameplayObject.GetComponent<MissionSummary>().missionComplete();
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
