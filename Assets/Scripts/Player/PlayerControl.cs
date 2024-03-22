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

    // cached values from PlayerInput
    private float _playerForward = 0f;
    private float _playerTurn = 0f;
    private float _playerLookX = 0f;
    private float _playerLookY = 0f;
    private bool _playerActionGrab = false;
    private bool _playerActionCrouch = false;
    private bool _playerIsSprint = false;

    private bool isCrouched = false;

    // enable/disable player control from inputs
    public bool isPlayerControlEnabled = true;

    // direction of the Cinemachine virtual 3rd person follow camera
    public GameObject cameraDir;
    public float LookCamSensitivityX = 5f;
    public float LookCamSensitivityY = 5f;

    // internal camera heading and pitch rotation state
    private Quaternion cameraHeading;
    private float cameraPitch;
    
    // limits for the camera pitch controls
    public float cameraPitchMax = 30f;
    public float cameraPitchMin = -20f;
    public bool invertMouseY = false;

    public float movementSpeedWalk = 1f;
    public float movementSpeedSprint = 2f;
    public float movementSpeedCrouch = 0.5f;

    // sprint stamina is depleted during sprinting at
    // this rate, represented in percentage per second.
    // For example, 0.2 == 20%
    // depletion from full to zero will take 5 seconds.
    public float sprintStaminaDepletionRate = 0.2f;

    // recharge rate for sprint stamina, represented in 
    // percentage per second.  For example, 0.1 == 10%
    // recharge from zero will take 10 seconds.
    public float sprintStaminaRechargeRate = 0.1f;

    // stamina for sprinting.  Value range 0 to 1
    public float sprintStamina = 1f;

    public GameEvent audioEventToRaise;
    public AudioClip sprintStaminaOutOfBreathAudioClip;

    /*
     * ScriptablObjects
     */

    public PlayerData playerData;

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

        if(sprintStaminaOutOfBreathAudioClip == null || audioEventToRaise == null)
        {
            Debug.LogError("PlayerControl: no audioEventToRaise, sprintStaminaOutOfBreath audio clip component.");
        }

        // initialize the cameraDir target to match the player's transform
        cameraDir.transform.rotation = transform.rotation;
        cameraDir.transform.position = transform.position;
        cameraHeading = transform.rotation;
        cameraPitch = 0;

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
        _playerLookY = input.playerLookY;
        _playerIsSprint = input.playerIsSprint;

        // don't overwrite the cached input buttons (if true, not yet handled)
        _playerActionGrab = input.playerActionGrab || _playerActionGrab;
        _playerActionCrouch = input.playerActionCrouch || _playerActionCrouch;

        if (playerData != null)
        {
            playerData.PlayerPosition = transform.position;
        }
        
    }

    private void FixedUpdate()
    {
        if (!isPlayerControlEnabled || cameraDir == null)
            return;

        
        // update the cinemachine camera based on inputs from mouse and right analog stick

        // calculate new input axes relative to the camera's offset 
        // from the player's forward vector
            
        {
            // the camera target gameobject follows the player
                
            Vector3 crouchTargetPosition = transform.position;
            if (isCrouched)
                crouchTargetPosition.y += 0.7f;
            else
                crouchTargetPosition.y += 1.4f;

            cameraDir.transform.position = crouchTargetPosition;

            // update the camera heading
            Quaternion rotX = Quaternion.AngleAxis(_playerLookX * LookCamSensitivityX, Vector3.up);
            cameraHeading *= rotX;

            // update the camera pitch
            cameraPitch += _playerLookY * LookCamSensitivityY;
            cameraPitch = Mathf.Clamp(cameraPitch, cameraPitchMin, cameraPitchMax);
            Quaternion rotY;
            if (invertMouseY)
                rotY = Quaternion.AngleAxis(cameraPitch, Vector3.left);
            else
                rotY = Quaternion.AngleAxis(cameraPitch, Vector3.right);

            cameraDir.transform.rotation = cameraHeading * rotY;
        }
                

        Vector3 inputXZ = Vector3.zero;
        if (isPlayerControlEnabled)
            inputXZ = new Vector3(_playerTurn, 0f, _playerForward);

        Quaternion headingOffsetFromCamera = Quaternion.Inverse(transform.rotation) * cameraHeading; // cameraDir.transform.rotation;

        // rotate the input hor/vert vector by the difference between the player's
        // heading and the current camera look offset
        inputXZ = headingOffsetFromCamera * inputXZ;

        // update the animation controller with the corrected forward and turn values
        // relative to the camera and player

        // update sprint stamina
        if (_playerIsSprint && inputXZ.magnitude >= 0.5f && sprintStamina != 0f)
        {
            // deplete stamina
            sprintStamina -= sprintStaminaDepletionRate * Time.fixedDeltaTime;

            if (sprintStamina <= 0f)
            {
                // fire off audio event that the player has run out of stamina
                audioEventToRaise.Raise(this, sprintStaminaOutOfBreathAudioClip, transform.position);
                Debug.Log("Stamina depleted!");
            }
            sprintStamina = Mathf.Clamp(sprintStamina, 0f, 1f);

            // update the PlayerData SO sprint stamina
            //
            //

        }
        if (!_playerIsSprint && sprintStamina < 1f)
        {
            // recharge stamina
            sprintStamina += sprintStaminaRechargeRate * Time.fixedDeltaTime;
            sprintStamina = Mathf.Clamp(sprintStamina, 0f, 1f);
            // update the PlayerData SO sprint stamina
            //
            //

        }


        float movementScaleFactor = 0.001f;
        float animControlVelY;
        if (isCrouched)
        {
            movementScaleFactor *= movementSpeedCrouch;
            animControlVelY = 0.25f;
        }
        else if (_playerIsSprint && sprintStamina > 0f)
        {
            movementScaleFactor *= movementSpeedSprint;
            animControlVelY = 1f;
        }
        else
        {
            movementScaleFactor *= movementSpeedWalk;
            animControlVelY = 0.3f;
        }
        // move the player using the forward vector component
        Vector3 flatPlayerForward = transform.forward;
        flatPlayerForward.y = 0f;
        flatPlayerForward.Normalize();
        Vector3 forwardMoveVec = flatPlayerForward / Time.fixedDeltaTime * inputXZ.z * movementScaleFactor;
            
        // move the player using the lateral vector component
        Vector3 lateralMoveVec = (Quaternion.AngleAxis(90f, Vector3.up) * flatPlayerForward).normalized /
                                    Time.fixedDeltaTime * inputXZ.x * movementScaleFactor;

        transform.position += forwardMoveVec + lateralMoveVec;

        // update the animation controller with correct forward velocity.
        // lateral velocity is zero currently, can add later if we want
        // turning walk/run animations
        anim.SetFloat("velx", 0f);// inputXZ.x);
        anim.SetFloat("vely", inputXZ.z * animControlVelY);

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

        if (_playerActionCrouch)
        {
            _playerActionCrouch = false;
            isCrouched = !isCrouched;
            anim.SetBool("crouch", isCrouched);
        }

        if (_playerActionGrab)
        {
            _playerActionGrab = false;
            if (stealableObject != null && isStealableObjectInRangetoSteal && !HasStolenObject)
            {
                stealableObject.SetActive(false);
                HasStolenObject = true;

                if (stealableObjectComponent != null && stealableObjectComponent.AudioClipSteal != null)
                    stealableObjectComponent.Steal();
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
