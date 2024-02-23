using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AIState
{ 
    Patrol,
    Seek
};

public class AI : MonoBehaviour
{
    // public
    [Header("Initialize AI State")]
    public AIState aistate;
    

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
        setNextWaypoint();
     }
    
    // Update is called once per frame
    void Update()
    {
        SynchronizeAnimatorAndAgent();
        switch (aistate)
        {
            case AIState.Patrol:
                if ((agent.remainingDistance < WaypointDistanceTolerance)&&!agent.pathPending)
                {
                    setNextWaypoint();
                }
                break;
            case AIState.Seek:
                break;
        }
    }
    private bool setNextWaypoint()
    {
        bool retval = false;
        currWaypoint++;
        //currWaypoint = curr;
        // loop back to 0
        if (currWaypoint >= waypoints.Length)
        {
            currWaypoint = 0;
            retval = true;
        }
        agent.SetDestination(waypoints[currWaypoint].transform.position);
        Debug.Log("Set the destionation to waypoint " + currWaypoint);
        return retval;
    }

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


    public void RequestBehaviorInvestigate()
    {
        Debug.Log("RequestBehaviorInvestigate Called");
    }

    public void RequestBehaviorPursuit()
    {
        Debug.Log("RequestBehaviorPursuit Called");
    }
    public void RequestBehaviorIdle()
    {
        Debug.Log("RequestBehaviorIdle Called");
    }
}