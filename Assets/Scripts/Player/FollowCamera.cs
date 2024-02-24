using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Add this script to a camera game object to smoothly follow 
a game object such as the player.  
*/

public class FollowCamera : MonoBehaviour
{
  // cameraFrom specifies the position of the camera relative to
  // the look-at object.  Easiest method is to place it in the player object's
  // GameObject hierarchy.  For rotation calculations, this will always be
  // referenced relative to the the cameraTarget's forward vector.
  public GameObject cameraFrom;

  // cameraTarget specifies the position of the camera's look-at object.
  // This object's forward vector should align with the player GameObject.
  public GameObject cameraTarget;

  // linear interpolation rates affect how fast the camera converges
  // on the player's position and forward vector
  private float translateRate = 3f;
  private float rotationRate = 3f;

  // goal position and rotation of camera such that it follows
  // and looks at the player's cameraTarget child object
  private Quaternion camGoalRot;
  private Vector3 camGoalPosFiltered;

  // filtered position value of cameraTarget position
  private Vector3 cameraTargetPosFiltered;


  // calculates and returns the camera look-at rotation.
  Quaternion getCameraRotation()
  {
    // bizarre that c# does not allow const for non-built-in types! 
    Vector3 upVec = new Vector3(0f, 1f, 0f);
    Vector3 camTargetDiffXZ = cameraTargetPosFiltered - camGoalPosFiltered;
    return Quaternion.LookRotation(camTargetDiffXZ, upVec);
  }

  void Start()
  {
    if (cameraFrom == null)
    {
      Debug.LogError("PlayerCamera: no cameraFrom.");
      return;
    }
    if (cameraTarget == null)
    {
      Debug.LogError("PlayerCamera: no cameraTarget.");
      return;
    }

    // initialize the camera position
    // the goal position is the cameraFrom GameObject
    camGoalPosFiltered = cameraFrom.transform.position;
    transform.position = camGoalPosFiltered;

    // initialize the camera rotation using authored 
    transform.rotation = cameraFrom.transform.rotation;
    camGoalRot = getCameraRotation();

    // initialize the filtered cameraTarget position
    cameraTargetPosFiltered = cameraTarget.transform.position;
  }


  void FixedUpdate()
  {
    if (!cameraFrom || !cameraTarget)
      return;

    // accumulation based low pass filter to calculate the new camera location.
    // the goal position is the cameraFrom GameObject
    float posFilterT = 0.1f;
    camGoalPosFiltered = camGoalPosFiltered * (1f - posFilterT) + cameraFrom.transform.position * posFilterT;
    transform.position = Vector3.LerpUnclamped(transform.position, camGoalPosFiltered, Time.deltaTime * translateRate);

    // also filter the camera target position to smooth out erroneous root motion artifacts
    cameraTargetPosFiltered = cameraTargetPosFiltered * (1f - posFilterT) + cameraTarget.transform.position * posFilterT;

    // calculate the camera rotation from its new filtered goal position
    // such that it looks at the cameraTarget GameObject
    camGoalRot = getCameraRotation();

    // interpolate towards the new camera rotation
    transform.rotation = Quaternion.LerpUnclamped(transform.rotation, camGoalRot, Time.deltaTime * rotationRate);

  }
}
