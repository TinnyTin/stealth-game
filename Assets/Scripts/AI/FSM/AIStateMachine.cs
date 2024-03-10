using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AIThreatPriority
{
    Idle,
    Investigate,
    Pursuit
}

[RequireComponent(typeof(ThreatMeter))]
[RequireComponent(typeof(FieldOfView))]
public class AIStateMachine : MonoBehaviour
{
    // configurable from inspector
    public float walkSpeed = 0.2f;
    public float runSpeed = 1.0f;
    public float currentSpeed = 0.2f;
    public float pursuitAutoSenseRadius = 2f;
    public float finalCaughtRadius = 1.5f;

    // public
    [Header("Initialize AI State")]
    public AIBaseState entryState;

    [Header("Idle Settings")]
    public float WaypointDistanceTolerance = 1.0f;
    public GameObject[] waypoints;

    [Header("Hostile Settings")]
    public Vector3 lastThreat;
    public int countInView;

    [Header("ScriptableObjects")]
    public AIData aiData;
    public GameEvent PlayerCaught;
    public GameEvent AudioChannel;

    [Header("AudioClips")]
    public AudioClip audioClipGasp;
    public AudioClip audioClipAlert;

    [HideInInspector]
    public AIThreatPriority aiThreatPriority;
    [HideInInspector]
    public FieldOfView _FOV;

    // state variables
    protected AIBaseState _currentState;
    protected AIStateFactory _states;

    // private
    [HideInInspector]
    public Animator anim;
    [HideInInspector]
    public NavMeshAgent agent;

    // navmeshagent velocity and movement smoothing
    private Vector2 _velocity;
    private Vector2 SmoothDeltaDirection;
    protected int _lastWaypointIdx = -1;

    public AIBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public int LastWaypointIdx { get { return _lastWaypointIdx; } set { _lastWaypointIdx = value; } }


    private void Awake()
    {

        // Register in AI Data SO
        aiData.RegisterAI(this.gameObject);

        // set up context
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        anim.applyRootMotion = true;
        agent.updatePosition = false;
        agent.updateRotation = false; // let rootmotion handle this

        // set up navmesh and waypoints
        LastWaypointIdx = -1;

        // setup state
        _states = new AIStateFactory(this);
        _currentState = _states.Waypoint(); // TODO replace this with configurable default state from inspector
        _currentState.SetAIThreatPriority();
        _currentState.EnterState();


    }

    private void Start()
    {
        _FOV = GetComponent<FieldOfView>();
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
        bool shouldMove;
        _velocity.x = 0f;
        _velocity.y = 0f;
        if (agent.path.corners == null)
        {
            shouldMove = false;
        }
        else
        {
            // delta position to the agent.next position
            Vector3 worldDeltaPosition = agent.path.corners[0] - transform.position;
            worldDeltaPosition.y = 0; // always synched b2ween agent and rootmotion on OnAnimatorMove()

            // local space delta (gameobject orientation)
            float dx = Vector3.Dot(transform.right, worldDeltaPosition);
            float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
            Vector2 deltaDirection = new Vector2(dx, dy).normalized;

            // non-framerate based smoothing for the delta position
            float smooth = Mathf.Min(1, Time.deltaTime / 0.1f);
            SmoothDeltaDirection = Vector2.Lerp(SmoothDeltaDirection, deltaDirection, smooth);


            // calculate velocity (based on smooth position / time)
            _velocity = SmoothDeltaDirection * currentSpeed;

            // smooth out velocity when approaching the stopping distance
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                _velocity = Vector2.Lerp(
                    Vector2.zero, _velocity, agent.remainingDistance / agent.stoppingDistance);
            }

            // transition from idle --> moving, and prevent overshooting stopping point and creating circling around again
            shouldMove = (_velocity.magnitude > 0.5f) && (agent.remainingDistance > agent.stoppingDistance);
        }


        // update animator parameters for rootmotion
        anim.SetBool("move", shouldMove);
        anim.SetFloat("velx", _velocity.x);
        anim.SetFloat("vely", _velocity.y);

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

    public void setSpeed(float speed)
    {
        agent.speed = speed;
        currentSpeed = speed;
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

    private void OnEnable()
    {
        aiData.RegisterAI(this.gameObject);
    }

    private void OnDisable()
    {
        aiData.UnRegisterAI(this.gameObject);
    }

    // ======================================================
    // Catch Callback
    // ======================================================
    public void CatchPlayer()
    {
        StartCoroutine("CoroutineCatchPlayer", 1.0f);
    }


    IEnumerator CoroutineCatchPlayer(float delay)
    {
        yield return new WaitForSeconds(delay);
        PlayerCaught.Raise();
    }
}


