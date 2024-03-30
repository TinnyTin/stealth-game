using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * CS6457 Attributions
 * Tiny Brain
 * Original Author:     Justin Wu
 * Contributors:
 * Description: Idle/Waypoint AI State class
 * External Source Credit: https://www.youtube.com/watch?v=kV06GiJgFhc&t=1535s
 *                      
 */

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
    public override bool SetAIThreatPriority()
    {
        Ctx.aiThreatPriority = AIThreatPriority.Idle;
        return true;
    }
    public override void EnterState()
    {
        currWaypoint = Ctx.LastWaypointIdx;
        waypoints = Ctx.waypoints;

        setNextWaypoint();
        Ctx.setSpeed(Ctx.walkSpeed);
        Ctx.agent.isStopped = false; 
    }
    public override void UpdateState()
    {
        if ((Ctx.agent.remainingDistance < WaypointDistanceTolerance) && !Ctx.agent.pathPending)
        {
            Ctx.OnWaypointReached(currWaypoint);
            setNextWaypoint();
        }
    }
    public override void ExitState()
    {
        Ctx.LastWaypointIdx = currWaypoint-1;
        Ctx.agent.isStopped = true; 
    }
    public override void CheckSwitchState()
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
