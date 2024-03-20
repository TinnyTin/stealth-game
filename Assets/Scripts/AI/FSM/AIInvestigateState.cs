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
    public override bool SetAIThreatPriority()
    {
        Ctx.aiThreatPriority = AIThreatPriority.Investigate;
        return true;
    }
    public override void EnterState()
    {

        // play surprised animation
        AIBaseState animationSurprised = Factory.animationSubState("Surprised", "triggerSurprised", Ctx.audioClipGasp);
        SwitchSubState(animationSurprised);
        
        Ctx.setSpeed(Ctx.walkSpeed);
        Ctx.AudioChannel.Raise(Ctx.audioClipGasp, Ctx.transform.position, AudioSourceParams.Default);
    }
    public override void UpdateState()
    {
        Ctx.agent.SetDestination(Ctx.lastThreat);
        if ((Ctx.agent.remainingDistance < 5) && !Ctx.agent.pathPending)
        {
            // loop LookAroundCut animation
            if (!AIAnimationSubState.CheckAnimationString(CurrentSubState,"Look"))
            { 
                AIBaseState animationLookAround = Factory.animationSubState("Look", "triggerLook", null);
                SwitchSubState(animationLookAround);
            }
        }
    }
    public override void ExitState()
    {
    }
    public override void CheckSwitchState()
    {

    }
    public override void InitializeSubState() { }

}
