using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIStateMachine : MonoBehaviour
{
    // configurable from inspector
    public float walkSpeed = 0.2f;
    public float runSpeed = 1.0f;

    // public
    [Header("Initialize AI State")]
    public AIBaseState entryState;

    [Header("Idle Settings")]
    public float WaypointDistanceTolerance = 1.0f;
    public GameObject[] waypoints;

    [Header("Hostile Settings")]
    public Vector3 lastThreat;

    // state variables
    protected AIBaseState _currentState;
    protected AIStateFactory _states;

    // private
    [HideInInspector]
    public Animator anim;
    [HideInInspector]
    public NavMeshAgent agent;

    // navmeshagent velocity and movement smoothing
    private Vector2 Velocity;
    private Vector2 SmoothDeltaPosition;

    public AIBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }


    private void Awake()
    {
        // set up context
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        anim.applyRootMotion = true;
        agent.updatePosition = false;
        agent.updateRotation = false; // let rootmotion handle this

        // setup state
        _states = new AIStateFactory(this);
        _currentState = _states.Waypoint(); // TODO replace this with configurable default state from inspector
        _currentState.EnterState();
    }

    private void Update()
    {
        SynchronizeAnimatorAndAgent();
        _currentState.UpdateStates();
        _currentState.CheckSwitchStates();
    }

    // ======================================================
    // Animation things
    // ======================================================

    // https://www.youtube.com/watch?v=uAGjKxH4sDQ
    private void SynchronizeAnimatorAndAgent()
    {

        // delta position to the agent.next position
        Vector3 worldDeltaPosition = agent.nextPosition - transform.position;
        worldDeltaPosition.y = 0; // always synched b2ween agent and rootmotion on OnAnimatorMove()

        // local space delta (gameobject orientation)
        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2(dx, dy);

        // non-framerate based smoothing for the delta position
        float smooth = Mathf.Min(1, Time.deltaTime / 0.1f);
        SmoothDeltaPosition = Vector2.Lerp(SmoothDeltaPosition, deltaPosition, smooth);


        // calculate velocity (based on smooth position / time)
        Velocity = SmoothDeltaPosition / Time.deltaTime;
        //Velocity = deltaPosition;

        // smooth out velocity when approaching the stopping distance
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            Velocity = Vector2.Lerp(
                Vector2.zero, Velocity, agent.remainingDistance / agent.stoppingDistance);
        }

        // transition from idle --> moving, and prevent overshooting stopping point and creating circling around again
        bool shouldMove = (Velocity.magnitude > 0.5f) && (agent.remainingDistance > agent.stoppingDistance);

        // update animator parameters for rootmotion
        anim.SetBool("move", shouldMove);
        anim.SetFloat("velx", Velocity.x);
        anim.SetFloat("vely", Velocity.y);

        // check if model and agent are too far away from each other.
        // prevents walking too far away from the navmesh
        float deltaMagnitude = worldDeltaPosition.magnitude;
        if (deltaMagnitude > agent.radius / 2f)
        {
            transform.position = Vector3.Lerp(
                anim.rootPosition,
                agent.nextPosition,
                smooth
                );
        }
    }

    private void OnAnimatorMove()
    {
        Vector3 rootPosition = anim.rootPosition;
        rootPosition.y = agent.nextPosition.y; // follow navmesh slopes

        // update transform position and rotation based on animation + navmesh height
        transform.position = rootPosition;
        transform.rotation = anim.rootRotation;

        // update navmesh agent with new animation root position
        agent.nextPosition = rootPosition;
    }

    // ======================================================
    // Switching Behavior
    // ======================================================

    public void BehaviorWaypoint()
    {
        _currentState.SwitchState(_states.Waypoint());
    }

    public void BehaviorInvestigate()
    {
        _currentState.SwitchState(_states.Investigate());
    }

    public void BehaviorPursuit()
    {
        _currentState.SwitchState(_states.Pursuit());
    }
}


