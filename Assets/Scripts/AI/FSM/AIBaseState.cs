/*
 * CS6457 Attributions
 * Tiny Brain
 * Original Author:     Justin Wu
 * Contributors:
 * Description: Abstract AI State class
 * External Source Credit: https://www.youtube.com/watch?v=kV06GiJgFhc&t=1535s
 *                      
 */
public abstract class AIBaseState
{
    private bool _isRootState = false;
    private AIStateMachine _ctx;
    private AIStateFactory _factory;
    private AIBaseState _currentSuperState;
    private AIBaseState _currentSubState;
    protected AIBaseState CurrentSubState { get { return _currentSubState; } }

    protected bool IsRootState { get { return _isRootState; } set { _isRootState = value; } }
    protected AIStateMachine Ctx { get { return _ctx; } }
    protected AIStateFactory Factory { get { return _factory; } }

    
    public AIBaseState(AIStateMachine currentContext, AIStateFactory aiStateFactory)
    {
        _ctx = currentContext;
        _factory = aiStateFactory;
    }
    public abstract bool SetAIThreatPriority();
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchState();
    // only implemented if you always plan on having a substate for a superstate.
    // if that is the case, call InitializeSubstate() in the parent's constructor.
    public abstract void InitializeSubState();


    public void UpdateStates()
    {
        UpdateState();
        if (_currentSubState != null)
        {
            _currentSubState.UpdateStates();
        }
    }

    public void CheckSwitchStates()
    {
        CheckSwitchState();
        if (_currentSubState != null)
        {
            _currentSubState.CheckSwitchStates();
        }
    }

    public void ExitStates()
    {
        if (_currentSubState != null)
        {
            _currentSubState.ExitStates();
        }
        ExitState();
    }

    public void SwitchState(AIBaseState newState)
    {
        // current state exits state
        ExitStates();

        // set threat priority
        if (newState.SetAIThreatPriority())
        {
            // update AI Manager
            if (Ctx.aiManager != null)
                Ctx.aiManager.UpdateThreatPriority();
        }

        // new state enters state
        newState.EnterState();

        if (_isRootState)
        {
            // switch current state of context
            _ctx.CurrentState = newState;
        }
        else if (_currentSuperState != null)
        {
            _currentSuperState.SetSubState(newState);
        }

    }
    protected void SetSuperState(AIBaseState newSuperState)
    {
        _currentSuperState = newSuperState;
    }
    private void SetSubState(AIBaseState newSubState)
    {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }
    
    // allow current State to switch its subState on the fly in updateState() or checkSwitchState()
    protected void SwitchSubState(AIBaseState newSubState)
    {
        if (_currentSubState != null)
        {
            _currentSubState.ExitState();
        }

        // set threat priority
        if (newSubState.SetAIThreatPriority())
        {
            // update AI Manager
            if (Ctx.aiManager != null)
                Ctx.aiManager.UpdateThreatPriority();
        }
        
        // new state enters state
        newSubState.EnterState();

        // set the substate
        SetSubState(newSubState);
    }

}