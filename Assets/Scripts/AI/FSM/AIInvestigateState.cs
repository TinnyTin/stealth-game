using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * CS6457 Attributions
 * Tiny Brain
 * Original Author:     Justin Wu
 * Contributors:
 * Description: Investigate AI State class
 * External Source Credit: https://www.youtube.com/watch?v=kV06GiJgFhc&t=1535s
 *                      
 */
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
        Ctx.setSpeed(Ctx.walkSpeed);
         Ctx.AudioChannel.Raise(Ctx.audioClipGasp, Ctx.transform.position, AudioSourceParams.Default);
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
