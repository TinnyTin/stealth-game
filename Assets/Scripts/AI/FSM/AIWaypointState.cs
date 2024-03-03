using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AIWaypointState : AIBaseState
{
    public float WaypointDistanceTolerance = 1.0f;
    public GameObject[] waypoints;

    private int currWaypoint = 0;

    public AIWaypointState(AIStateMachine currentContext, AIStateFactory aiStateFactory) : base(currentContext, aiStateFactory)
    {
        IsRootState = true;
        InitializeSubState();
    }
    public override void EnterState()
    {
        currWaypoint = -1;
        waypoints = Ctx.waypoints;

        setNextWaypoint();
    }
    public override void UpdateState()
    {
        if ((Ctx.agent.remainingDistance < WaypointDistanceTolerance) && !Ctx.agent.pathPending)
        {
            setNextWaypoint();
        }
    }
    public override void ExitState() { }
    public override void CheckSwitchStates()
    {

    }
    public override void InitializeSubState() { }

    /// ======================================================
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
        Ctx.agent.SetDestination(waypoints[currWaypoint].transform.position);
        //Debug.Log("Set the destination to waypoint " + currWaypoint);
        return retval;
    }
}
