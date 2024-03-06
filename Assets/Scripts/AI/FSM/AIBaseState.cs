public abstract class AIBaseState
{
    private bool _isRootState = false;
    private AIStateMachine _ctx;
    private AIStateFactory _factory;
    private AIBaseState _currentSuperState;
    private AIBaseState _currentSubState;

    protected bool IsRootState { get { return _isRootState; } set { _isRootState = value; } }
    protected AIStateMachine Ctx { get { return _ctx; } }
    protected AIStateFactory Factory { get { return _factory; } }
    public AIBaseState(AIStateMachine currentContext, AIStateFactory aiStateFactory)
    {
        _ctx = currentContext;
        _factory = aiStateFactory;
    }
    public abstract void SetAIThreatPriority();
    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchStates();
    public abstract void InitializeSubState();

    public void UpdateStates()
    {
        UpdateState();
        if (_currentSubState != null)
        {
            _currentSubState.UpdateStates();
        }
    }

    public void ExitStates()
    {
        ExitState();
        if (_currentSubState != null)
        {
            _currentSubState.ExitStates();
        }
    }

    public void SwitchState(AIBaseState newState)
    {
        // current state exits state
        ExitState();

        // set threat priority
        newState.SetAIThreatPriority();

        // new state enters state
        newState.EnterState();

        // update AI Manager
        if (Ctx.aiData != null)
            Ctx.aiData.UpdateThreatPriority();

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
    protected void SetSubState(AIBaseState newSubState)
    {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }

    protected void CheckOverallSwitchState()
    {
        //TODO based on threatmeter can switch states here
    }
}