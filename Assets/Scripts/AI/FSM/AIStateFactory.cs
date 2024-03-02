using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
