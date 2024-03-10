using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/*
 * CS6457 Attributions
 * Tiny Brain
 * Original Author:     Erik
 * Contributors:        
 */

/*
 * Add this script to the player object and set the camera target to smoothly 
 * follow the player.  
 */

public class FollowCamera : MonoBehaviour
{
  // the active camera in the scene
  public GameObject followCam;

  // cameraFrom specifies the position of the camera relative to
  // the look-at object.  Easiest method is to place it in the player object's
  // GameObject hierarchy.  For rotation calculations, this will always be
  // referenced relative to the the cameraTarget's forward vector.
  private GameObject cameraFrom;

  // cameraTarget specifies the position of the camera's look-at object.
  // This object's forward vector should align with the player GameObject.
  private GameObject cameraTarget;

  // linear interpolation rates affect how fast the camera converges
  // on the player's position and forward vector
  public float posConvergeRate = 8f;
  public float rotConvergeRate = 20f;

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

  public void Initialize()
  {
    if (followCam == null)
    {
      Debug.LogError("PlayerCamera: no camera");
      return;
    }
    // get the from/to follow camera objects in the player object's hierarchy
    cameraFrom = GameObject.Find("CameraFrom");
    cameraTarget = GameObject.Find("CameraTarget");

    if (cameraFrom == null)
    {
      Debug.LogError("PlayerCamera: no cameraFrom");
      return;
    }
    if (cameraTarget == null)
    {
      Debug.LogError("PlayerCamera: no cameraTarget");
      return;
    }

    // initialize the camera position
    // the goal position is the cameraFrom GameObject
    camGoalPosFiltered = cameraFrom.transform.position;
    followCam.transform.position = camGoalPosFiltered;

    // initialize the camera rotation using authored 
    camGoalRot = getCameraRotation();
    followCam.transform.rotation = camGoalRot;

    // initialize the filtered cameraTarget position
    cameraTargetPosFiltered = cameraTarget.transform.position;

  }

  void Start()
  {
    Initialize();
  }


  void FixedUpdate()
  {
    if (!cameraFrom || !cameraTarget)
      return;

    // accumulation based low pass filter to calculate the new camera location.
    // the goal position is the cameraFrom GameObject
    float posFilterT = 0.3f;
    camGoalPosFiltered = Vector3.Lerp(camGoalPosFiltered, cameraFrom.transform.position, posFilterT);
    followCam.transform.position = Vector3.LerpUnclamped(followCam.transform.position, camGoalPosFiltered, Time.deltaTime * posConvergeRate);

    // also filter the camera target position to smooth out erroneous root motion artifacts
    cameraTargetPosFiltered = Vector3.Lerp(cameraTargetPosFiltered, cameraTarget.transform.position, posFilterT);

    // calculate the camera rotation from its new filtered goal position
    // such that it looks at the cameraTarget GameObject
    camGoalRot = getCameraRotation();

    // interpolate towards the new camera rotation
    followCam.transform.rotation = Quaternion.LerpUnclamped(followCam.transform.rotation, camGoalRot, Time.deltaTime * rotConvergeRate);

  }
}
