using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * CS6457 Attributions
 * Tiny Brain
 * Original Author:     Justin Wu
 * Contributors:        Tom
 * Description: Investigate AI State class
 * External Source Credit: https://www.youtube.com/watch?v=kV06GiJgFhc&t=1535s
 *                      
 */
public class AIInvestigateState_Wife : AIBaseState
{
    private AIStateMachine_Wife _ctxWife = null; 

    public AIInvestigateState_Wife(AIStateMachine currentContext, AIStateFactory aiStateFactory) : base(currentContext, aiStateFactory)
    {
        IsRootState = true;
        InitializeSubState();

        if (Ctx is AIStateMachine_Wife ctxWife)
            _ctxWife = ctxWife;
    }
    public override bool SetAIThreatPriority()
    {
        Ctx.aiThreatPriority = AIThreatPriority.Investigate;
        return true;
    }
    public override void EnterState()
    {
        // play thinking animation
        AIBaseState animationThinking = Factory.animationSubState("Thinking", "triggerThinking", Ctx.audioClipGasp, true);
        SwitchSubState(animationThinking);

        Ctx.setSpeed(Ctx.walkSpeed);
        AudioSourceParams audioSourceParams = new();
        audioSourceParams.MinDistance = 250;
        Ctx.AudioChannel.Raise(Ctx.audioClipGasp, Ctx.transform.position, audioSourceParams);

        // Begin growing the FOV view radius
        if (_ctxWife != null)
            Ctx.FOV.GrowFOVAngle(_ctxWife.maxFOVAngle, _ctxWife.growFOVAngleTime);
    }
    public override void UpdateState()
    {
        // Feature to find target if you're in pursuit State and the target is already close by
        Vector3? targetpos = Ctx.FOV.FindTargetWithinRadius(Ctx.pursuitAutoSenseRadius);
        if (targetpos != null)
        {
            Ctx.lastThreat = (Vector3)targetpos;
        }

        // pursue Target
        Ctx.agent.SetDestination(Ctx.lastThreat);

        if ((Ctx.agent.remainingDistance < 5) && !Ctx.agent.pathPending)
        {
            // loop Look animation
            if (!AIAnimationSubState.CheckAnimationString(CurrentSubState, "Look"))
            {
                Ctx.FOV.updateTransform = false;
                AIBaseState animationLookAround = Factory.animationSubState("Look", "triggerLook", null, true);
                SwitchSubState(animationLookAround);
            }
        }

        // exit out of animation lock if the target is in view or a sound is made
        if (((Ctx.countInView != 0) || (targetpos.HasValue)))
            if (!AIAnimationSubState.CheckAnimationString(CurrentSubState, "Thinking"))
                SwitchSubState(Factory.EmptySubState());
    }
    public override void ExitState()
    {
        Ctx.FOV.updateTransform = true;
        
        // Begin shrinking the FOV view radius
        if (_ctxWife != null)
            Ctx.FOV.ShrinkFOVAngle(_ctxWife.minFOVAngle, _ctxWife.shrinkFOVAngleTime);
    }
    public override void CheckSwitchState()
    {

    }
    public override void InitializeSubState() { }

}
