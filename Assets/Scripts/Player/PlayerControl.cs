using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

/*
 * CS6457 Attributions
 * Tiny Brain
 * Original Author:     Erik
 * Contributors:        Justin
 */

/*
 * Main player script for controlling player behavior and follow 
 * camera from player input
 */

[RequireComponent(typeof(Animator), typeof(Rigidbody), typeof(PlayerInput))]
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
    public FootStepFactory footStepFactory;
    private bool leftFootDown, rightFootDown;
    private Vector3 prevPosition;
    private float velocityFiltered;

    // joints in the player skeleton rig for footstep calculations
    public GameObject rigBase;
    public GameObject leftFoot;
    public GameObject rightFoot;

    // if true, enable mouse / right analog stick camera control
    public bool lookCamEnabled;

    // cached values from PlayerInput
    private float _playerForward = 0f;
    private float _playerTurn = 0f;
    private float _playerLookX = 0f;

    private bool _playerActionGrab = false;
    private bool _playerActionCrouch = false;

    private bool isCrouched = false;

    public bool isPlayerControlEnabled = true;


    // direction of the Cinemachine virtual 3rd person follow camera
    public GameObject cameraDir;
    public float LookCamSensitivity = 5f;

    public float movementSpeedWalk = 1f;
    public float movementSpeedRun = 2f;
    public float movementSpeedCrouch = 0.5f;

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

        // the Cinemachine camera requires a target CameraDir gameobject
        if (cameraDir == null)
        {
            Debug.LogError("PlayerControl: no CameraDir component.");
        }

        // initialize the cameraDir target to match the player's transform
        cameraDir.transform.rotation = transform.rotation;
        cameraDir.transform.position = transform.position;

        if (rigBase == null || leftFoot == null || rightFoot == null)
        {
            Debug.LogError("PlayerControl: rigBase, leftFoot, rightFoot not set.");
        }
        leftFootDown = false;
        rightFootDown = false;
        prevPosition = transform.position;
        velocityFiltered = 0f;

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
        if (cameraDir)
        {
            // update the cinemachine camera based on inputs from mouse and right analog stick

            // calculate new input axes relative to the camera's offset 
            // from the player's forward vector

            // the camera target gameobject follows the player
            cameraDir.transform.position = transform.position;

            Quaternion rot = Quaternion.AngleAxis(_playerLookX * LookCamSensitivity, Vector3.up);
            if (isPlayerControlEnabled)
            {
                cameraDir.transform.rotation *= rot;
            }

            Vector3 inputXZ = Vector3.zero;
            if (isPlayerControlEnabled)
                inputXZ = new Vector3(_playerTurn, 0f, _playerForward);

            Quaternion headingOffsetFromCamera = Quaternion.Inverse(transform.rotation) * cameraDir.transform.rotation;

            // rotate the input hor/vert vector by the difference between the player's
            // heading and the current camera look offset
            inputXZ = headingOffsetFromCamera * inputXZ;

            // update the animation controller with the corrected forward and turn values
            // relative to the camera and player
            anim.SetFloat("velx", 0f);// inputXZ.x);
            anim.SetFloat("vely", inputXZ.z);

            float movementScaleFactor = 0.001f;
            if (isCrouched)
                movementScaleFactor *= movementSpeedCrouch;
            else
                movementScaleFactor *= movementSpeedWalk;

            // move the player using the forward vector component
            Vector3 forwardMoveVec = transform.forward / Time.fixedDeltaTime * inputXZ.z * movementScaleFactor;
            
            // move the player using the lateral vector component
            Vector3 lateralMoveVec = (Quaternion.AngleAxis(90f, Vector3.up) * transform.forward).normalized /
                                     Time.fixedDeltaTime * inputXZ.x * movementScaleFactor;

            transform.position += forwardMoveVec + lateralMoveVec;


            // now rotate the player toward the desired input direction. 
            // try doing it directly, later add interpolation

            // get the relative rotation of the input vector to the camera
            // here, z is forward
            Vector3 inputXZHeading = new Vector3(_playerTurn, 0f, _playerForward);
            float inputRot = Vector3.SignedAngle(Vector3.forward, inputXZHeading.normalized, Vector3.up);
            Quaternion inputRotQuat = Quaternion.AngleAxis(inputRot, Vector3.up);

            // mouse-look when not in motion.  if moving, mouse X will change forward vector
            if (inputXZHeading.magnitude > 0.2f)
                transform.rotation = cameraDir.transform.rotation * inputRotQuat;
        }


        if (_playerActionCrouch)
        {
            //anim.SetBool("doButtonPress", _playerActionCrouch);
            _playerActionCrouch = false;
            isCrouched = !isCrouched;
            Debug.Log("Crouch: " + isCrouched);
            anim.SetBool("crouch", isCrouched);
        }

        if (_playerActionGrab)
        {
            _playerActionGrab = false;
            Debug.Log("Grab item");
            if (stealableObject != null && isStealableObjectInRangetoSteal && !HasStolenObject)
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
            if (!isStealableObjectInRangeToHilight && stealableObjectComponent != null)
                stealableObjectComponent.SetHilight(true);

            isStealableObjectInRangeToHilight = true;
        }
        else
        {
            if (isStealableObjectInRangeToHilight && stealableObjectComponent != null)
                stealableObjectComponent.SetHilight(false);
            isStealableObjectInRangeToHilight = false;
        }

        if (distance < 2f)
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
        /*
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
        */
    }

    // emit footstep event for left and right foot
    // if below y threshold
    private void triggerFootsteps(bool isRunning)
    {
        StepCharacteristic stepCharacteristic = isRunning ? StepCharacteristic.Run : StepCharacteristic.Walk;
        if (rigBase && leftFoot)
        {
            float leftFootY = leftFoot.transform.position.y - rigBase.transform.position.y;
            //Debug.Log(rigBase.transform.position.y + " " + leftFoot.transform.position.y + " " + leftFootY);
            if (leftFootY < 0.18f)
            {
                // only emit footstep one time once y threshold is passed.
                if (!leftFootDown)
                {
                    footStepFactory.playFootStepRandom(FloorCharacteristic.Dirt, stepCharacteristic, transform.position);
                }
                leftFootDown = true;
            }
            else
            {
                leftFootDown = false;
            }
        }

        if (rigBase && rightFoot)
        {
            float rightFootY = rightFoot.transform.position.y - rigBase.transform.position.y;

            if (rightFootY < 0.18f)
            {
                // only emit footstep one time once y threshold is passed.
                if (!rightFootDown)
                {
                    //Debug.Log("r foot: " + rbody.velocity.magnitude);
                    footStepFactory.playFootStepRandom(FloorCharacteristic.Dirt, stepCharacteristic, transform.position);
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
