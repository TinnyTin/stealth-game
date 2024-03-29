using UnityEngine;

/*
 * CS6457 Attributions
 * Tiny Brain
 * Original Author:     Justin Wu
 * Contributors:        Tom
 * Description: AI State Factory
 * External Source Credit: https://www.youtube.com/watch?v=kV06GiJgFhc&t=1535s
 *                      
 */
public class AIStateFactory
{
    AIStateMachine _context;
    AIWaypointState _waypointState;
    AIInvestigateState _investigateState;
    AIInvestigateState_Wife _wifeInvestigateState;
    AIPursuitState _pursuitState;
    AIPursuitState_Wife _wifePursuitState;
    AIEmptySubState _emptySubState;
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
    public AIBaseState WifeInvestigate()
    {
        if (_wifePursuitState == null)
        {
            _wifeInvestigateState = new AIInvestigateState_Wife(_context, this);
        }
        return _wifeInvestigateState;
    }
    public AIBaseState Pursuit()
    {
        if (_pursuitState == null)
        {
            _pursuitState = new AIPursuitState(_context, this);
        }
        return _pursuitState;
    }
    public AIBaseState WifePursuit()
    {
        if (_wifePursuitState == null)
        {
            _wifePursuitState = new AIPursuitState_Wife(_context, this);
        }
        return _wifePursuitState;
    }

    public AIAnimationSubState animationSubState(string strAnimation, string strTrigger, AudioClip audioClip, bool freezeFOV)
    {
        return new AIAnimationSubState(_context, this, strAnimation, strTrigger, audioClip, freezeFOV);
    }

    public AIBaseState EmptySubState()
    {
        if (_emptySubState == null)
        {
            _emptySubState = new AIEmptySubState(_context, this);
        }
        return _emptySubState;
    }
}
