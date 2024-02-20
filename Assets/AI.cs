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
    public AIState aistate;
    public float WaypointDistanceTolerance = 1.0f;
    
    public GameObject[] waypoints;
    
    // private
    private Animator anim;
    private NavMeshAgent agent;
    //private waypoints
    private int currWaypoint = 0;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        currWaypoint = -1;
        setNextWaypoint();
    }

    // Update is called once per frame
    void Update()
    {
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
        return retval;
    }

    private void SynchronizeAnimatorAndAgent()
    {
        //anim.SetBool("move", agent.velocity.magnitude > 0.5f);
        //anim.SetFloat("velx",agent.velocity.magnitude dot side vector)
        //anim.SetFloat("velx",agent.velocity.magnitude dot forward vector)
    }
}
