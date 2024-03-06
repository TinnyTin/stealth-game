using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIInvestigateState : AIBaseState
{
    public AIInvestigateState(AIStateMachine currentContext, AIStateFactory aiStateFactory) : base(currentContext, aiStateFactory)
    {
        IsRootState = true;
        InitializeSubState();
    }
    public override void SetAIThreatPriority()
    {
        Ctx.aiThreatPriority = AIThreatPriority.Investigate;
    }
    public override void EnterState()
    {
        Ctx.anim.SetTrigger("triggerSurprised");
    }
    public override void UpdateState()
    {
        Ctx.agent.SetDestination(Ctx.lastThreat);
        if ((Ctx.agent.remainingDistance < 5) && !Ctx.agent.pathPending)
        {
            if (!Ctx.anim.GetCurrentAnimatorStateInfo(0).IsName("LookAroundCut"))
            {
                Ctx.anim.SetTrigger("triggerLook");
            }
        }
    }
    public override void ExitState()
    {
        Ctx.anim.ResetTrigger("triggerSurprised");
        Ctx.anim.ResetTrigger("triggerLook");
    }
    public override void CheckSwitchStates()
    {

    }
    public override void InitializeSubState() { }

}
