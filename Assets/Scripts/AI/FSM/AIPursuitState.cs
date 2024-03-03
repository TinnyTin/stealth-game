public class AIPursuitState : AIBaseState
{
    private float lastSpeed;
    public AIPursuitState(AIStateMachine currentContext, AIStateFactory aiStateFactory) : base(currentContext, aiStateFactory)
    {
        IsRootState = true;
        InitializeSubState();
    }
    public override void EnterState()
    {
        lastSpeed = Ctx.agent.speed;
        Ctx.agent.speed = Ctx.runSpeed;
    }
    public override void UpdateState()
    {
        Ctx.agent.SetDestination(Ctx.lastThreat);
        if ((Ctx.agent.remainingDistance < 3) && !Ctx.agent.pathPending)
        {
            if (!Ctx.anim.GetCurrentAnimatorStateInfo(0).IsName("Angry Point"))
            {
                Ctx.anim.SetTrigger("triggerAngry");
            }
        }
    }
    public override void ExitState()
    {
        Ctx.agent.speed = lastSpeed;
        Ctx.anim.ResetTrigger("triggerAngry");
    }
    public override void CheckSwitchStates()
    {

    }
    public override void InitializeSubState() { }

}
