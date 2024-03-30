using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
 * CS6457 Attributions
 * Tiny Brain
 * Original Author:     Justin Wu
 * Contributors:        Tom
 * Description: AI State Machine + Context for AI state functionality - Civilian specific
 * External Source Credit: https://www.youtube.com/watch?v=kV06GiJgFhc&t=1535s
 *
 */

[AddComponentMenu("AI State Machine - Civilian")]
public class AIStateMachine_Civilian : AIStateMachine
{
    [System.Serializable]
    private class WaypointAction
    {
        public bool enabled = true; 

        [Tooltip("Waypoint in waypoints list that the action applies to. If there is no corresponding waypoint the action will be ignored.")]
        [Min(0)]
        public int waypointIndex = 0;

        [Tooltip("Name of the animator state to transition to. State must have corresponding enter and exit trigger.")]
        public string animStateName = string.Empty;
        
        [Tooltip("Time to stay in state in seconds.")]
        [Min(0)]
        public float stateHoldTime = 1;

        [Tooltip("Probability that the idle state transition will occur. 0 = never occurs, 1 = always occurs.")]
        [Range(0.0f, 1.0f)]
        public float stateEnterProbability = 1.0f; 
    }

    [System.Serializable]
    private enum CivilianType
    {
        [Tooltip("Civilian will loop through their idle animation state forever.")]
        Idle,
        [Tooltip("Civilian will navigate waypoints set in Waypoints list above. Civilian will optionally " +
                 "suspend their path to complete and idle animation if any are set in Waypoint Actions below.")]
        WaypointsAndIdle
    }

    [Header("----- Derived Class Fields -----")]
    [Space]
    [SerializeField]
    private CivilianType _civilianType = CivilianType.Idle;

    [Space]
    [SerializeField]
    private string _idleAnimStateName;

    [Space]
    [SerializeField]
    private List<WaypointAction> _waypointActions;

    private int _lastWaypointReached = -1;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    protected override void SetInitialState()
    {
        
        if (_civilianType == CivilianType.Idle)
            _currentState = _states.Idle_Civilian(_idleAnimStateName);
        else if (_civilianType == CivilianType.WaypointsAndIdle)
            _currentState = _states.Waypoint(); 

        _currentState.EnterState();
    }

    public override void OnWaypointReached(int waypointIndex)
    {
        WaypointAction waypointAction = _waypointActions.FirstOrDefault(e => e.waypointIndex == waypointIndex);

        if (waypointAction != null && _lastWaypointReached != waypointIndex && waypoints.Length > waypointIndex && waypointAction.enabled)
        {
            float prob = Random.Range(0f, 1f);
            if (prob < waypointAction.stateEnterProbability)
            {
                _currentState.SwitchState(_states.Idle_Civilian(waypointAction.animStateName));
                StartCoroutine(CoroutineReturnToWaypoint(waypointAction.stateHoldTime));
            }
        }

        _lastWaypointReached = waypointIndex;
    }

    IEnumerator CoroutineReturnToWaypoint(float delay)
    {
        yield return new WaitForSeconds(delay);
        _currentState.SwitchState(_states.Waypoint());
    }
}
