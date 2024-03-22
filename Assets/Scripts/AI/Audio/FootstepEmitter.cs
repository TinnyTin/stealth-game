using UnityEngine;

/*
 * CS6457 Attributions
 * Tiny Brain
 * Original Author:    Tom 
 * Contributors:        
 */

public class FootstepEmitter : MonoBehaviour
{
    [SerializeField] private FootStepFactory _footStepFactory;

    [Space]
    [Tooltip("Sets the height above the ground at which a footstep sound should be played.")]
    [SerializeField] private float _footYEpsilon = 0.18f; 

    // joints in the player skeleton rig for footstep calculations
    [Space]
    [Header("Rig References")]
    [SerializeField] private GameObject _rigBase;
    [SerializeField] private GameObject _leftFoot;
    [SerializeField] private GameObject _rightFoot;

    // data for footstep tracking and sound event emitter
    private bool _leftFootDown;
    private bool _rightFootDown;
    private Vector3 _prevPosition;
    private float _velocityFiltered;

    // Start is called before the first frame update
    void Start()
    {
        if (_rigBase == null || _leftFoot == null || _rightFoot == null)
            Debug.LogWarning("FootstepEmitter: rigBase or leftFoot or rightFoot not set. Will not be able to raise generate footstep sounds.");

        _leftFootDown = false;
        _rightFootDown = false;
        _prevPosition = transform.position;
        _velocityFiltered = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        // calculate world-space velocity, magnitude is used to determine what 
        // sound to emit for footsteps
        float velocityCur = (transform.position - _prevPosition).magnitude / Time.fixedDeltaTime;

        _velocityFiltered = Mathf.Lerp(_velocityFiltered, velocityCur, 0.3f);
        _prevPosition = transform.position;
        bool isRunning = _velocityFiltered > 2f;

        // emit footstep events to sound manager
        TriggerFootsteps(isRunning);
    }

    // emit footstep event for left and right foot
    // if below y threshold
    private void TriggerFootsteps(bool isRunning)
    {
        StepCharacteristic stepCharacteristic = isRunning ? StepCharacteristic.Run : StepCharacteristic.Walk;
        if (_rigBase && _leftFoot)
        {
            float leftFootY = _leftFoot.transform.position.y - _rigBase.transform.position.y;
            //Debug.Log(rigBase.transform.position.y + " " + leftFoot.transform.position.y + " " + leftFootY);
            if (leftFootY < _footYEpsilon)
            {
                // only emit footstep one time once y threshold is passed.
                if (!_leftFootDown)
                    _footStepFactory.playFootStepRandom(FloorCharacteristic.Dirt, stepCharacteristic, transform.position);

                _leftFootDown = true;
            }
            else
            {
                _leftFootDown = false;
            }
        }

        if (_rigBase && _rightFoot)
        {
            float rightFootY = _rightFoot.transform.position.y - _rigBase.transform.position.y;

            if (rightFootY < _footYEpsilon)
            {
                // only emit footstep one time once y threshold is passed.
                if (!_rightFootDown)
                    _footStepFactory.playFootStepRandom(FloorCharacteristic.Dirt, stepCharacteristic, transform.position);

                _rightFootDown = true;
            }
            else
            {
                _rightFootDown = false;
            }
        }
    }
}
