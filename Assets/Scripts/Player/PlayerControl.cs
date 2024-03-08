using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Rigidbody), typeof(PlayerInput))]
[RequireComponent(typeof(FollowCamera))]
public class PlayerControl : MonoBehaviour
{
  // stealable object
  private GameObject stealableObject;
  private StealableObject stealableObjectComponent;
  private bool isStealableObjectInRangetoSteal = false;
  private bool isStealableObjectInRangeToHilight = false;

  // highlight the target object when the player is within range
  public float HilightRangeStealableObject = 5f;

  public bool HasStolenObject = false;

  // extraction point
  private GameObject extractionPointObject;
  private ExtractionPoint extractionPointComponent;
  private bool isExtractionPointInRangeToHilight = false;

  // highlight the target object when the player is within range
  public float HilightRangeExtractionPoint = 7f;

  public bool IsExtractionSuccess = false;


  private Rigidbody rbody;
  private Animator anim;
  private PlayerInput input;

  // data for footstep tracking and sound event emitter
  private FootstepEmitter footsteps;
  bool leftFootDown, rightFootDown;
  Vector3 prevPosition;
  float velocityFiltered;

  public float LookCamSensitivity = 3f;

  // if true, enable mouse / right analog stick camera control
  public bool LookCamEnabled;

  // cached values from PlayerInput
  private float _playerForward = 0f;
  private float _playerTurn = 0f;
  private float _playerLookX = 0f;

  private bool _playerActionGrab = false;
  private bool _playerActionCrouch = false;

  private bool isCrouched = false;

  public bool isPlayerControlEnabled = true;

  // camera transforms, must be in the player gameobject hierarchy
  private GameObject cameraFrom;
  private GameObject cameraTarget;

  // current world space camera roation
  // used to calculate camera transforms when LookCamEnabled

  // store the zero rotation camera from position offset
  private Vector3 camPosZeroOffset;

  // camera look offset from the initial camera rotation
  // (directly behind, looking at player is zero)
  private Quaternion camLookOffset;

  // store initial player game object rotation, which must 
  // be used as an offset for all later rotation calculations
  private Quaternion playerInitialRot;


  public void Initialize()
  {
    // get stealable object and component
    stealableObject = GameObject.Find("StealableObject");
    if (stealableObject == null)
    {
      Debug.LogError("PlayerControl: no StealableObject found.");
    }
    stealableObjectComponent = stealableObject.GetComponent<StealableObject>();
    if (stealableObjectComponent == null)
    {
      Debug.LogError("PlayerControl: StealableObject has no StealableObject component.");
    }

    // get extraction point object and component
    extractionPointObject = GameObject.Find("ExtractionPoint");
    if (extractionPointObject == null)
    {
      Debug.LogError("PlayerControl: no ExtractionPoint found.");
    }
    extractionPointComponent = extractionPointObject.GetComponent<ExtractionPoint>();
    if (extractionPointComponent == null)
    {
      Debug.LogError("PlayerControl: ExtractionPoint has no ExtractionPoint component.");
    }

    rbody = GetComponent<Rigidbody>();
    if (rbody == null)
    {
      Debug.LogError("PlayerControl: no RigidBody component.");
    }
    anim = GetComponent<Animator>();
    if (anim == null)
    {
      Debug.LogError("PlayerControl: no Animator component.");
    }

    input = GetComponent<PlayerInput>();
    if (input == null)
    {
      Debug.LogError("PlayerControl: no PlayerInput component.");
    }

    // get the from/to follow camera objects in the player object's hierarchy
    cameraFrom = GameObject.Find("CameraFrom");
    cameraTarget = GameObject.Find("CameraTarget");
    if (cameraFrom == null || cameraTarget == null)
    {
      Debug.LogError("PlayerControl: no CameraFrom or CameraTo component.");
    }

    // cache the original camera offset from its target attached to the player
    camPosZeroOffset = cameraFrom.transform.position - cameraTarget.transform.position;

    // zero out the look camera offset rotation (mouse / right analog stick look)
    //camLookOffset = transform.rotation;
    playerInitialRot = transform.rotation;

    camLookOffset = new Quaternion(0, 0, 0, 1);

    // get the footstep emitter and initialize the footstep tracking state
    footsteps = GetComponent<FootstepEmitter>();
    if (footsteps == null)
    {
      Debug.LogError("PlayerControl: no FootstepEmitter component.");
    }
    leftFootDown = false;
    rightFootDown = false;
    prevPosition = transform.position;
    velocityFiltered = 0f;

    // reinit the follow camera script
    GetComponent<FollowCamera>().Initialize();

    isPlayerControlEnabled = true;
  }


  void Start()
  {
    Initialize();
  }


  // Update is called once per frame
  void Update()
  {
    // cache the player input parameters
    _playerForward = input.playerForward;
    _playerTurn = input.playerTurn;
    _playerLookX = input.playerLookX;

    // don't overwrite the cached input buttons (if true, not yet handled)
    _playerActionGrab = input.playerActionGrab || _playerActionGrab;
    _playerActionCrouch = input.playerActionCrouch || _playerActionCrouch;
  }

  private void FixedUpdate()
  {
    if (LookCamEnabled && cameraFrom && cameraTarget)
    {
      // update the look camera based on inputs from mouse and right analog stick

      // calculate new input axes relative to the camera's offset 
      // from the player's forward vector

      Vector3 camDiff = cameraFrom.transform.position - cameraTarget.transform.position;
      Vector3 upVec = new Vector3(0f, 1f, 0f);
      Quaternion rot = Quaternion.AngleAxis(_playerLookX * LookCamSensitivity, upVec);
      if(isPlayerControlEnabled)
        camLookOffset *= rot;

      camDiff = camLookOffset * camPosZeroOffset;
      cameraFrom.transform.position = cameraTarget.transform.position + camDiff;
      Vector3 inputXY = new Vector3(0f, 0f, 0f);
      if(isPlayerControlEnabled)
        inputXY = new Vector3(_playerTurn, 0f, _playerForward);

      // rotate the input hor/vert vector by the difference between the player's heading and
      // the current camera look offset
      inputXY = playerInitialRot * camLookOffset * Quaternion.Inverse(cameraTarget.transform.rotation) * inputXY;

      _playerTurn = inputXY.x;
      _playerForward = inputXY.z;

      // direct backwards movement doesn't work well in the blend tree, so
      // offset with a turn if it occurs
      if(_playerForward < -0.4f && Mathf.Abs(_playerTurn) < 0.05f)
      {
        _playerTurn = 1f;
      }
    }

    //Debug.Log(_playerTurn + " " +  _playerForward);
    anim.SetFloat("velx", _playerTurn);
    anim.SetFloat("vely", _playerForward);

    if (_playerActionCrouch)
    {
      //anim.SetBool("doButtonPress", _playerActionCrouch);
      _playerActionCrouch = false;
      isCrouched = !isCrouched;
      Debug.Log("Crouch: " + isCrouched);
      anim.SetBool("crouch", isCrouched);
    }

    if(_playerActionGrab)
    {
      _playerActionGrab = false;
      Debug.Log("Grab item");
      if (stealableObject != null && isStealableObjectInRangetoSteal)
      {
        stealableObject.SetActive(false);
        HasStolenObject = true;

        if (stealableObjectComponent != null && stealableObjectComponent.AudioClipSteal != null)
          stealableObjectComponent.Steal();

        // fire off an event indicating that the object is stolen
        //
        //
      }
    }

    // calculate world-space velocity, magnitude is used to determine what 
    // sound to emit for footsteps
    float velocityCur = (transform.position - prevPosition).magnitude / Time.fixedDeltaTime;

    velocityFiltered = Mathf.Lerp(velocityFiltered, velocityCur, 0.3f);
    prevPosition = transform.position;
    bool isRunning = false;
    if (velocityFiltered > 2f)
      isRunning = true;

    // emit footstep events to sound manager
    triggerFootsteps(isRunning);

    // update the stealable object state 
    UpdateStealableObject();

    // update the extraction state 
    UpdateExtractionPoint();
  }

  void UpdateStealableObject()
  {
    if (stealableObject == null || stealableObjectComponent == null)
      return;

    Vector3 positionDiff = transform.position - stealableObject.transform.position;
    positionDiff.y = 0f;
    float distance = Mathf.Abs(positionDiff.magnitude);
    
    if (distance < HilightRangeStealableObject)
    {
      if(!isStealableObjectInRangeToHilight && stealableObjectComponent != null)
        stealableObjectComponent.SetHilight(true);

      isStealableObjectInRangeToHilight = true;
    }
    else
    {
      if (isStealableObjectInRangeToHilight && stealableObjectComponent != null)
        stealableObjectComponent.SetHilight(false);
      isStealableObjectInRangeToHilight = false;
    }

    if(distance < 2f)
      isStealableObjectInRangetoSteal = true;

  }

  void UpdateExtractionPoint()
  {
    if (!HasStolenObject)
      return;

    if (extractionPointObject == null || extractionPointComponent == null)
      return;

    Vector3 positionDiff = transform.position - extractionPointObject.transform.position;
    positionDiff.y = 0f;
    float distance = Mathf.Abs(positionDiff.magnitude);
    if (distance < HilightRangeExtractionPoint)
    {
      if (!isExtractionPointInRangeToHilight)
        extractionPointComponent.SetHilight(true);

      isExtractionPointInRangeToHilight = true;
    }
    else
    {
      if (isExtractionPointInRangeToHilight)
        extractionPointComponent.SetHilight(false);

      isExtractionPointInRangeToHilight = false;
    }

    if (distance < 1.5f && !IsExtractionSuccess)
    {
      IsExtractionSuccess = true;
      extractionPointComponent.Extract();
      Debug.Log("Extraction: success.");
      // fire off extraction success event to manager 
      // 
      //
    }
  }

  // manage the root motion depending on animator state and input
  void OnAnimatorMove()
  {
    Quaternion newRot = anim.rootRotation;
    Vector3 newPos = anim.rootPosition;

    Vector3 newRotEuler = newRot.eulerAngles;
    newRotEuler.x = 0f; 
    newRotEuler.z = 0f;
    newRot.eulerAngles = newRotEuler;

    this.transform.position = newPos;
    this.transform.rotation = newRot;

    rbody.MovePosition(newPos);
    rbody.MoveRotation(newRot);
  }

  // emit footstep event for left and right foot
  // if below y threshold
  private void triggerFootsteps(bool isRunning)
  {
    GameObject rigBase = GameObject.Find("rig-main");
    GameObject leftFoot = GameObject.Find("LeftFoot");
    if (rigBase && leftFoot)
    {
      float leftFootY = leftFoot.transform.position.y - rigBase.transform.position.y;
      //Debug.Log(rigBase.transform.position.y + " " + leftFoot.transform.position.y + " " + leftFootY);
      if (leftFootY < 0.18f)
      {
        // only emit footstep one time once y threshold is passed.
        if (!leftFootDown)
        {
          footsteps.EmitFootstep(isRunning);
        }
        leftFootDown = true;
      }
      else
      {
        leftFootDown = false;
      }
    }
    GameObject rightFoot = GameObject.Find("RightFoot");
    if (rigBase && leftFoot)
    {
      float rightFootY = rightFoot.transform.position.y - rigBase.transform.position.y;

      if (rightFootY < 0.18f)
      {
        // only emit footstep one time once y threshold is passed.
        if (!rightFootDown)
        {
          //Debug.Log("r foot: " + rbody.velocity.magnitude);
          footsteps.EmitFootstep(isRunning);
        }
        rightFootDown = true;
      }
      else
      {
        rightFootDown = false;
      }
    }
  }

}
