using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * CS6457 Attributions
 * Tiny Brain
 * Original Author:     Justin Wu
 * Contributors:
 * Description: AI State Factory
 * External Source Credit: https://www.youtube.com/watch?v=kV06GiJgFhc&t=1535s
 *                      
 */
public class AIStateFactory
{
    AIStateMachine _context;
    AIWaypointState _waypointState;
    AIInvestigateState _investigateState;
    AIPursuitState _pursuitState;
    public AIStateFactory(AIStateMachine currentContext)
    {
        _context = currentContext;
    }

    public AIBaseState Waypoint()
    {
        if (_waypointState == null)
        {
            _waypointState = new AIWaypointState(_context, this);
        }
        return _waypointState;
    }
    public AIBaseState Investigate()
    {
        if (_investigateState == null)
        {
            _investigateState = new AIInvestigateState(_context, this);
        }
        return _investigateState;
    }
    public AIBaseState Pursuit()
    {
        if (_pursuitState == null)
        {
            _pursuitState = new AIPursuitState(_context, this);
        }
        return _pursuitState;
    }
}
