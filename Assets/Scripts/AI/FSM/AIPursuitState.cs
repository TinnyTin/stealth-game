using UnityEngine;

public class AIPursuitState : AIBaseState
{
    private float lastSpeed;
    public AIPursuitState(AIStateMachine currentContext, AIStateFactory aiStateFactory) : base(currentContext, aiStateFactory)
    {
        IsRootState = true;
        InitializeSubState();
    }
    public override void SetAIThreatPriority()
    {
        Ctx.aiThreatPriority = AIThreatPriority.Pursuit;
    }
    public override void EnterState()
    {
        lastSpeed = Ctx.agent.speed;
        Ctx.setSpeed(Ctx.runSpeed);
    }
    public override void UpdateState()
    {
        // Feature to find target if you're in pursuit State and the target is already close by
        Vector3? targetpos = Ctx._FOV.FindTargetWithinRadius(Ctx.pursuitAutoSenseRadius);
        if (targetpos != null)
        {
            Ctx.lastThreat = (Vector3)targetpos;
            Ctx.agent.SetDestination(Ctx.lastThreat);
        }

        // pursue Target
        Ctx.agent.SetDestination(Ctx.lastThreat);
        if ((Ctx.agent.remainingDistance < 1.5) && !Ctx.agent.pathPending)
        {
            // if the target is no longer in view, need to find the guy
            if ((Ctx.countInView == 0) && (targetpos == null))
            {
                if (!Ctx.anim.GetCurrentAnimatorStateInfo(0).IsName("LookAroundCut"))
                {
                    Ctx.anim.SetTrigger("triggerLook");
                }
            }
            else if (!Ctx.anim.GetCurrentAnimatorStateInfo(0).IsName("Angry Point"))
            {
                Ctx.anim.SetTrigger("triggerAngry");
                Ctx.PlayerCaught.Raise(Ctx);
            }

        }




    }
    public override void ExitState()
    {
        Ctx.setSpeed(lastSpeed);
        Ctx.anim.ResetTrigger("triggerLook");
        Ctx.anim.ResetTrigger("triggerAngry");
    }
    public override void CheckSwitchStates()
    {

    }
    public override void InitializeSubState() { }


}
