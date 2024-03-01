using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    // public
    [Header("Initialize AI State")]
    public AIState aistate;
    // private idleState idlestate;
    private investigateState investigatestate;
    private pursuitState pursuitstate;


    [Header("Idle Settings")]
    public float WaypointDistanceTolerance = 1.0f;
    public GameObject[] waypoints;

    [Header("Hostile Settings")]
    public Vector3 lastThreat;

    // private
    private Animator anim;
    [HideInInspector]
    public NavMeshAgent agent;

    // navmeshagent velocity and movement smoothing
    private Vector2 Velocity;
    private Vector2 SmoothDeltaPosition;

    // waypoints
    private int currWaypoint = 0;

    // use awake() for object self-initialization, vs start() for communication to other gameobjects
    private void Awake()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        anim.applyRootMotion = true;
        agent.updatePosition = false;
        agent.updateRotation = false; // let rootmotion handle this

        currWaypoint = -1;
        investigatestate = investigateState.surprised;
        setNextWaypoint();
    }

    // Update is called once per frame
    void Update()
    {
        SynchronizeAnimatorAndAgent();
        aiFSM();
    }

    // ======================================================
    // AI FSM / behavior things
    // ======================================================

    public void RequestBehaviorInvestigate()
    {
        aistate = AIState.Investigate;
        Debug.Log("Investigate Called");
    }

    public void RequestBehaviorPursuit()
    {
        Debug.Log("Pursuit Called");
    }
    public void RequestBehaviorIdle()
    {
        setNextWaypoint(currWaypoint);
        aistate = AIState.Patrol;
        Debug.Log("Idle Called");
    }

    private void aiFSM()
    {
        switch (aistate)
        {
            case AIState.Patrol:
                if ((agent.remainingDistance < WaypointDistanceTolerance) && !agent.pathPending)
                {
                    setNextWaypoint();
                }
                break;
            case AIState.Investigate:

                investigateFSM();
                break;
        }
    }

    private void investigateFSM()
    {
        switch (investigatestate)
        {
            case investigateState.surprised:
                anim.SetTrigger("triggerSurprised");
                investigatestate = investigateState.approach;
                break;
            case investigateState.approach:
                agent.SetDestination(lastThreat);
                if ((agent.remainingDistance < 5) && !agent.pathPending)
                {
                    investigatestate = investigateState.surprised;
                    RequestBehaviorIdle();
                }
                break;
        }
    }

    private void pursuitFSM()
    {
        switch (pursuitstate)
        {
            case pursuitState.pursuit:
                break;
        }
    }

    // guard
    // lost - defeated
    // lost - research
    // alarm from other AI

    // civilian
    // more idle movement types
    // scared, scream
    // find nearest guard
    // throw garbage at player

    // ======================================================
    // Waypoint things
    // ======================================================
    private bool setNextWaypoint()
    {
        return setNextWaypoint(++currWaypoint);
    }

    private bool setNextWaypoint(int idx)
    {
        bool retval = false;
        currWaypoint = idx;
        // loop back to 0
        if (currWaypoint >= waypoints.Length)
        {
            currWaypoint = 0;
            retval = true;
        }
        agent.SetDestination(waypoints[currWaypoint].transform.position);
        //Debug.Log("Set the destination to waypoint " + currWaypoint);
        return retval;
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

}


public enum AIState
{
    Patrol,
    Investigate
};

public enum investigateState
{
    surprised,
    approach
};
public enum pursuitState
{
    pursuit
};